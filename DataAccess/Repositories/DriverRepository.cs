using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class DriverRepository
    {
        public Driver? GetDriverById(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, s.first_name, s.last_name, s.phone, s.address, s.position, 
                                   s.hire_date, s.salary, s.is_active as staff_active, v.registration_number,
                                   v.make, v.model
                                   FROM drivers d 
                                   INNER JOIN staff s ON d.staff_id = s.staff_id 
                                   LEFT JOIN vehicles v ON d.current_vehicle_id = v.vehicle_id
                                   WHERE d.driver_id = @driverId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    LicenseNumber = reader["license_number"].ToString() ?? string.Empty,
                                    LicenseType = reader["license_type"].ToString() ?? string.Empty,
                                    LicenseExpiryDate = Convert.ToDateTime(reader["license_expiry_date"]),
                                    ExperienceYears = Convert.ToInt32(reader["experience_years"]),
                                    VehicleTypePreference = reader["vehicle_type_preference"].ToString(),
                                    IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                    CurrentVehicleId = reader["current_vehicle_id"] != DBNull.Value ? 
                                        Convert.ToInt32(reader["current_vehicle_id"]) : null,
                                    Rating = Convert.ToDecimal(reader["rating"]),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Staff = new Staff
                                    {
                                        StaffId = Convert.ToInt32(reader["staff_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        Address = reader["address"].ToString() ?? string.Empty,
                                        Position = reader["position"].ToString() ?? string.Empty,
                                        HireDate = Convert.ToDateTime(reader["hire_date"]),
                                        Salary = Convert.ToDecimal(reader["salary"]),
                                        IsActive = Convert.ToBoolean(reader["staff_active"])
                                    },
                                    CurrentVehicle = reader["current_vehicle_id"] != DBNull.Value ? new Vehicle
                                    {
                                        VehicleId = Convert.ToInt32(reader["current_vehicle_id"]),
                                        RegistrationNumber = reader["registration_number"].ToString() ?? string.Empty,
                                        Make = reader["make"].ToString() ?? string.Empty,
                                        Model = reader["model"].ToString() ?? string.Empty
                                    } : null
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver: {ex.Message}");
                }
            }
            return null;
        }

        public List<Driver> GetAllDrivers()
        {
            var drivers = new List<Driver>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, s.first_name, s.last_name, s.phone, s.position, s.is_active as staff_active 
                                   FROM drivers d 
                                   INNER JOIN staff s ON d.staff_id = s.staff_id 
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            drivers.Add(new Driver
                            {
                                DriverId = Convert.ToInt32(reader["driver_id"]),
                                StaffId = Convert.ToInt32(reader["staff_id"]),
                                LicenseNumber = reader["license_number"].ToString() ?? string.Empty,
                                LicenseType = reader["license_type"].ToString() ?? string.Empty,
                                LicenseExpiryDate = Convert.ToDateTime(reader["license_expiry_date"]),
                                ExperienceYears = Convert.ToInt32(reader["experience_years"]),
                                VehicleTypePreference = reader["vehicle_type_preference"].ToString(),
                                IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                CurrentVehicleId = reader["current_vehicle_id"] != DBNull.Value ? 
                                    Convert.ToInt32(reader["current_vehicle_id"]) : null,
                                Rating = Convert.ToDecimal(reader["rating"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                Staff = new Staff
                                {
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Position = reader["position"].ToString() ?? string.Empty,
                                    IsActive = Convert.ToBoolean(reader["staff_active"])
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving drivers: {ex.Message}");
                }
            }
            return drivers;
        }

        public List<Driver> GetAvailableDrivers()
        {
            var drivers = new List<Driver>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, s.first_name, s.last_name, s.phone, s.position, s.is_active as staff_active 
                                   FROM drivers d 
                                   INNER JOIN staff s ON d.staff_id = s.staff_id 
                                   WHERE d.is_available = TRUE AND s.is_active = TRUE
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            drivers.Add(new Driver
                            {
                                DriverId = Convert.ToInt32(reader["driver_id"]),
                                StaffId = Convert.ToInt32(reader["staff_id"]),
                                LicenseNumber = reader["license_number"].ToString() ?? string.Empty,
                                LicenseType = reader["license_type"].ToString() ?? string.Empty,
                                LicenseExpiryDate = Convert.ToDateTime(reader["license_expiry_date"]),
                                ExperienceYears = Convert.ToInt32(reader["experience_years"]),
                                VehicleTypePreference = reader["vehicle_type_preference"].ToString(),
                                IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                CurrentVehicleId = reader["current_vehicle_id"] != DBNull.Value ? 
                                    Convert.ToInt32(reader["current_vehicle_id"]) : null,
                                Rating = Convert.ToDecimal(reader["rating"]),
                                Staff = new Staff
                                {
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Position = reader["position"].ToString() ?? string.Empty,
                                    IsActive = Convert.ToBoolean(reader["staff_active"])
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving available drivers: {ex.Message}");
                }
            }
            return drivers;
        }

        public int AddDriver(Driver driver)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO drivers (staff_id, license_number, license_type, license_expiry_date, 
                                   experience_years, vehicle_type_preference, is_available, current_vehicle_id, rating) 
                                   VALUES (@staffId, @licenseNumber, @licenseType, @licenseExpiryDate, @experienceYears, 
                                   @vehicleTypePreference, @isAvailable, @currentVehicleId, @rating);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@staffId", driver.StaffId);
                        command.Parameters.AddWithValue("@licenseNumber", driver.LicenseNumber);
                        command.Parameters.AddWithValue("@licenseType", driver.LicenseType);
                        command.Parameters.AddWithValue("@licenseExpiryDate", driver.LicenseExpiryDate);
                        command.Parameters.AddWithValue("@experienceYears", driver.ExperienceYears);
                        command.Parameters.AddWithValue("@vehicleTypePreference", driver.VehicleTypePreference ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isAvailable", driver.IsAvailable);
                        command.Parameters.AddWithValue("@currentVehicleId", driver.CurrentVehicleId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@rating", driver.Rating);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding driver: {ex.Message}");
                }
            }
        }

        public void UpdateDriver(Driver driver)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE drivers SET license_number = @licenseNumber, license_type = @licenseType, 
                                   license_expiry_date = @licenseExpiryDate, experience_years = @experienceYears, 
                                   vehicle_type_preference = @vehicleTypePreference, is_available = @isAvailable, 
                                   current_vehicle_id = @currentVehicleId, rating = @rating, updated_at = CURRENT_TIMESTAMP 
                                   WHERE driver_id = @driverId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@licenseNumber", driver.LicenseNumber);
                        command.Parameters.AddWithValue("@licenseType", driver.LicenseType);
                        command.Parameters.AddWithValue("@licenseExpiryDate", driver.LicenseExpiryDate);
                        command.Parameters.AddWithValue("@experienceYears", driver.ExperienceYears);
                        command.Parameters.AddWithValue("@vehicleTypePreference", driver.VehicleTypePreference ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isAvailable", driver.IsAvailable);
                        command.Parameters.AddWithValue("@currentVehicleId", driver.CurrentVehicleId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@rating", driver.Rating);
                        command.Parameters.AddWithValue("@driverId", driver.DriverId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating driver: {ex.Message}");
                }
            }
        }

        public void DeleteDriver(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM drivers WHERE driver_id = @driverId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting driver: {ex.Message}");
                }
            }
        }

        public bool LicenseNumberExists(string licenseNumber, int? excludeDriverId = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM drivers WHERE license_number = @licenseNumber";
                    if (excludeDriverId.HasValue)
                        query += " AND driver_id != @excludeDriverId";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@licenseNumber", licenseNumber);
                        if (excludeDriverId.HasValue)
                            command.Parameters.AddWithValue("@excludeDriverId", excludeDriverId);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking license number existence: {ex.Message}");
                }
            }
        }

        public void UpdateDriverAvailability(int driverId, bool isAvailable)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE drivers SET is_available = @isAvailable, updated_at = CURRENT_TIMESTAMP WHERE driver_id = @driverId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@isAvailable", isAvailable);
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating driver availability: {ex.Message}");
                }
            }
        }
    }
}