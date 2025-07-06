using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        public Driver? GetDriverById(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, u.username, u.email 
                                   FROM drivers d 
                                   LEFT JOIN users u ON d.user_id = u.user_id 
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
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Address = reader["address"].ToString() ?? string.Empty,
                                    LicenseNumber = reader["license_number"].ToString() ?? string.Empty,
                                    LicenseExpiryDate = Convert.ToDateTime(reader["license_expiry_date"]),
                                    HireDate = Convert.ToDateTime(reader["hire_date"]),
                                    IsActive = Convert.ToBoolean(reader["is_active"]),
                                    Status = reader["status"].ToString() ?? "available",
                                    VehicleAssignment = reader["vehicle_assignment"].ToString() ?? string.Empty,
                                    User = new User
                                    {
                                        UserId = Convert.ToInt32(reader["user_id"]),
                                        Username = reader["username"].ToString() ?? string.Empty,
                                        Email = reader["email"].ToString() ?? string.Empty
                                    }
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

        public Driver? GetDriverByUserId(int userId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, u.username, u.email 
                                   FROM drivers d 
                                   LEFT JOIN users u ON d.user_id = u.user_id 
                                   WHERE d.user_id = @userId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Address = reader["address"].ToString() ?? string.Empty,
                                    LicenseNumber = reader["license_number"].ToString() ?? string.Empty,
                                    LicenseExpiryDate = Convert.ToDateTime(reader["license_expiry_date"]),
                                    HireDate = Convert.ToDateTime(reader["hire_date"]),
                                    IsActive = Convert.ToBoolean(reader["is_active"]),
                                    Status = reader["status"].ToString() ?? "available",
                                    VehicleAssignment = reader["vehicle_assignment"].ToString() ?? string.Empty,
                                    User = new User
                                    {
                                        UserId = Convert.ToInt32(reader["user_id"]),
                                        Username = reader["username"].ToString() ?? string.Empty,
                                        Email = reader["email"].ToString() ?? string.Empty
                                    }
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver by user ID: {ex.Message}");
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
                    string query = @"SELECT d.*, u.username, u.email 
                                   FROM drivers d 
                                   LEFT JOIN users u ON d.user_id = u.user_id 
                                   ORDER BY d.first_name, d.last_name";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Address = reader["address"].ToString() ?? string.Empty,
                                    LicenseNumber = reader["license_number"].ToString() ?? string.Empty,
                                    LicenseExpiryDate = Convert.ToDateTime(reader["license_expiry_date"]),
                                    HireDate = Convert.ToDateTime(reader["hire_date"]),
                                    IsActive = Convert.ToBoolean(reader["is_active"]),
                                    Status = reader["status"].ToString() ?? "available",
                                    VehicleAssignment = reader["vehicle_assignment"].ToString() ?? string.Empty,
                                    User = new User
                                    {
                                        UserId = Convert.ToInt32(reader["user_id"]),
                                        Username = reader["username"].ToString() ?? string.Empty,
                                        Email = reader["email"].ToString() ?? string.Empty
                                    }
                                });
                            }
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

        public List<Driver> GetActiveDrivers()
        {
            var drivers = new List<Driver>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, u.username, u.email 
                                   FROM drivers d 
                                   LEFT JOIN users u ON d.user_id = u.user_id 
                                   WHERE d.is_active = true
                                   ORDER BY d.first_name, d.last_name";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Status = reader["status"].ToString() ?? "available",
                                    VehicleAssignment = reader["vehicle_assignment"].ToString() ?? string.Empty
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving active drivers: {ex.Message}");
                }
            }
            return drivers;
        }

        public List<Driver> GetDriversByStatus(string status)
        {
            var drivers = new List<Driver>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT d.*, u.username, u.email 
                                   FROM drivers d 
                                   LEFT JOIN users u ON d.user_id = u.user_id 
                                   WHERE d.status = @status AND d.is_active = true
                                   ORDER BY d.first_name, d.last_name";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Status = reader["status"].ToString() ?? "available",
                                    VehicleAssignment = reader["vehicle_assignment"].ToString() ?? string.Empty
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving drivers by status: {ex.Message}");
                }
            }
            return drivers;
        }

        public int AddDriver(Driver driver)
        {
            ArgumentNullException.ThrowIfNull(driver);
            
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO drivers (user_id, first_name, last_name, phone, address, 
                                   license_number, license_expiry_date, hire_date, is_active, status, vehicle_assignment) 
                                   VALUES (@userId, @firstName, @lastName, @phone, @address, @licenseNumber, 
                                   @licenseExpiryDate, @hireDate, @isActive, @status, @vehicleAssignment);
                                   SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", driver.UserId);
                        command.Parameters.AddWithValue("@firstName", driver.FirstName);
                        command.Parameters.AddWithValue("@lastName", driver.LastName);
                        command.Parameters.AddWithValue("@phone", driver.Phone);
                        command.Parameters.AddWithValue("@address", driver.Address);
                        command.Parameters.AddWithValue("@licenseNumber", driver.LicenseNumber);
                        command.Parameters.AddWithValue("@licenseExpiryDate", driver.LicenseExpiryDate);
                        command.Parameters.AddWithValue("@hireDate", driver.HireDate);
                        command.Parameters.AddWithValue("@isActive", driver.IsActive);
                        command.Parameters.AddWithValue("@status", driver.Status);
                        command.Parameters.AddWithValue("@vehicleAssignment", driver.VehicleAssignment);
                        
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
            ArgumentNullException.ThrowIfNull(driver);
            
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE drivers SET first_name = @firstName, last_name = @lastName, 
                                   phone = @phone, address = @address, license_number = @licenseNumber, 
                                   license_expiry_date = @licenseExpiryDate, is_active = @isActive, 
                                   status = @status, vehicle_assignment = @vehicleAssignment 
                                   WHERE driver_id = @driverId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", driver.FirstName);
                        command.Parameters.AddWithValue("@lastName", driver.LastName);
                        command.Parameters.AddWithValue("@phone", driver.Phone);
                        command.Parameters.AddWithValue("@address", driver.Address);
                        command.Parameters.AddWithValue("@licenseNumber", driver.LicenseNumber);
                        command.Parameters.AddWithValue("@licenseExpiryDate", driver.LicenseExpiryDate);
                        command.Parameters.AddWithValue("@isActive", driver.IsActive);
                        command.Parameters.AddWithValue("@status", driver.Status);
                        command.Parameters.AddWithValue("@vehicleAssignment", driver.VehicleAssignment);
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

        public void UpdateDriverStatus(int driverId, string status)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE drivers SET status = @status WHERE driver_id = @driverId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating driver status: {ex.Message}");
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
                    string query = "UPDATE drivers SET is_active = false WHERE driver_id = @driverId";

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

        public bool DriverExists(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM drivers WHERE driver_id = @driverId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking driver existence: {ex.Message}");
                }
            }
        }

        public bool IsDriverAvailable(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT status FROM drivers WHERE driver_id = @driverId AND is_active = true";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        var status = command.ExecuteScalar()?.ToString();
                        return string.Equals(status, "available", StringComparison.OrdinalIgnoreCase);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking driver availability: {ex.Message}");
                }
            }
        }

        public void AssignVehicleToDriver(int driverId, string vehicleAssignment)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE drivers SET vehicle_assignment = @vehicleAssignment WHERE driver_id = @driverId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@vehicleAssignment", vehicleAssignment);
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error assigning vehicle to driver: {ex.Message}");
                }
            }
        }
    }
}