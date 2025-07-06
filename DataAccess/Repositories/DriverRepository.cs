using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using eShiftManagementSystem.DataAccess.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        // Basic CRUD Operations
        public List<Staff> GetAllDrivers()
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.position LIKE '%driver%' OR s.position LIKE '%Driver%'
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
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

        public Staff? GetDriverById(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.staff_id = @driverId AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%')";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapStaffFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver by ID: {ex.Message}");
                }
            }
            return null;
        }

        public Staff? GetDriverByUserId(int userId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.user_id = @userId AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%')";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapStaffFromReader(reader);
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

        public int CreateDriver(Staff driver)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO staff (user_id, first_name, last_name, phone, address, position, hire_date, salary, is_active) 
                                   VALUES (@userId, @firstName, @lastName, @phone, @address, @position, @hireDate, @salary, @isActive);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", driver.UserId);
                        command.Parameters.AddWithValue("@firstName", driver.FirstName);
                        command.Parameters.AddWithValue("@lastName", driver.LastName);
                        command.Parameters.AddWithValue("@phone", driver.Phone);
                        command.Parameters.AddWithValue("@address", driver.Address);
                        command.Parameters.AddWithValue("@position", driver.Position);
                        command.Parameters.AddWithValue("@hireDate", driver.HireDate);
                        command.Parameters.AddWithValue("@salary", driver.Salary);
                        command.Parameters.AddWithValue("@isActive", driver.IsActive);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error creating driver: {ex.Message}");
                }
            }
        }

        public void UpdateDriver(Staff driver)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE staff SET user_id = @userId, first_name = @firstName, last_name = @lastName, 
                                   phone = @phone, address = @address, position = @position, hire_date = @hireDate, 
                                   salary = @salary, is_active = @isActive WHERE staff_id = @staffId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", driver.UserId);
                        command.Parameters.AddWithValue("@firstName", driver.FirstName);
                        command.Parameters.AddWithValue("@lastName", driver.LastName);
                        command.Parameters.AddWithValue("@phone", driver.Phone);
                        command.Parameters.AddWithValue("@address", driver.Address);
                        command.Parameters.AddWithValue("@position", driver.Position);
                        command.Parameters.AddWithValue("@hireDate", driver.HireDate);
                        command.Parameters.AddWithValue("@salary", driver.Salary);
                        command.Parameters.AddWithValue("@isActive", driver.IsActive);
                        command.Parameters.AddWithValue("@staffId", driver.StaffId);
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
                    // Check if driver has active assignments
                    string checkQuery = "SELECT COUNT(*) FROM transport_units WHERE driver_id = @driverId AND status IN ('assigned', 'in_progress')";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@driverId", driverId);
                        int activeAssignments = Convert.ToInt32(checkCommand.ExecuteScalar());
                        if (activeAssignments > 0)
                        {
                            throw new Exception("Cannot delete driver with active job assignments.");
                        }
                    }

                    // Soft delete by setting is_active to false
                    string query = "UPDATE staff SET is_active = false WHERE staff_id = @driverId";
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
                    string query = "SELECT COUNT(*) FROM staff WHERE staff_id = @driverId AND (position LIKE '%driver%' OR position LIKE '%Driver%')";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking if driver exists: {ex.Message}");
                }
            }
        }

        // Driver-Specific Queries
        public List<Staff> GetActiveDrivers()
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.is_active = true AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%')
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
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

        public List<Staff> GetAvailableDrivers()
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   LEFT JOIN transport_units tu ON s.staff_id = tu.driver_id AND tu.status IN ('assigned', 'in_progress')
                                   WHERE s.is_active = true AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%') 
                                   AND tu.driver_id IS NULL
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
                            }
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

        public List<Staff> GetDriversByPosition(string position)
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.position = @position
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@position", position);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving drivers by position: {ex.Message}");
                }
            }
            return drivers;
        }

        public Staff? GetDriverByPhone(string phone)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.phone = @phone AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%')";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@phone", phone);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapStaffFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver by phone: {ex.Message}");
                }
            }
            return null;
        }

        public List<Staff> SearchDrivers(string searchTerm)
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE (s.position LIKE '%driver%' OR s.position LIKE '%Driver%') 
                                   AND (s.first_name LIKE @searchTerm OR s.last_name LIKE @searchTerm 
                                        OR s.phone LIKE @searchTerm OR s.position LIKE @searchTerm)
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error searching drivers: {ex.Message}");
                }
            }
            return drivers;
        }

        // Assignment and Availability Management
        public bool IsDriverAvailable(int driverId, DateTime startDate, DateTime endDate)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM transport_units tu 
                                   INNER JOIN jobs j ON tu.job_id = j.job_id 
                                   WHERE tu.driver_id = @driverId AND tu.status IN ('assigned', 'in_progress')
                                   AND ((j.requested_pickup_date BETWEEN @startDate AND @endDate) 
                                        OR (j.requested_delivery_date BETWEEN @startDate AND @endDate)
                                        OR (@startDate BETWEEN j.requested_pickup_date AND COALESCE(j.requested_delivery_date, j.requested_pickup_date))
                                        OR (@endDate BETWEEN j.requested_pickup_date AND COALESCE(j.requested_delivery_date, j.requested_pickup_date)))";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                        return Convert.ToInt32(command.ExecuteScalar()) == 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking driver availability: {ex.Message}");
                }
            }
        }

        public List<Staff> GetDriversAvailableForDateRange(DateTime startDate, DateTime endDate)
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT DISTINCT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.is_active = true AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%')
                                   AND s.staff_id NOT IN (
                                       SELECT DISTINCT tu.driver_id FROM transport_units tu 
                                       INNER JOIN jobs j ON tu.job_id = j.job_id 
                                       WHERE tu.driver_id IS NOT NULL AND tu.status IN ('assigned', 'in_progress')
                                       AND ((j.requested_pickup_date BETWEEN @startDate AND @endDate) 
                                            OR (j.requested_delivery_date BETWEEN @startDate AND @endDate)
                                            OR (@startDate BETWEEN j.requested_pickup_date AND COALESCE(j.requested_delivery_date, j.requested_pickup_date))
                                            OR (@endDate BETWEEN j.requested_pickup_date AND COALESCE(j.requested_delivery_date, j.requested_pickup_date)))
                                   )
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving drivers available for date range: {ex.Message}");
                }
            }
            return drivers;
        }

        public void AssignDriverToJob(int driverId, int jobId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    
                    // Check if a transport unit already exists for this job
                    string checkQuery = "SELECT transport_unit_id FROM transport_units WHERE job_id = @jobId LIMIT 1";
                    int? transportUnitId = null;
                    
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@jobId", jobId);
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            transportUnitId = Convert.ToInt32(result);
                        }
                    }

                    if (transportUnitId.HasValue)
                    {
                        // Update existing transport unit
                        string updateQuery = "UPDATE transport_units SET driver_id = @driverId, status = 'assigned' WHERE transport_unit_id = @transportUnitId";
                        using (var updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@driverId", driverId);
                            updateCommand.Parameters.AddWithValue("@transportUnitId", transportUnitId.Value);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Create new transport unit
                        string insertQuery = "INSERT INTO transport_units (job_id, driver_id, status) VALUES (@jobId, @driverId, 'assigned')";
                        using (var insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@jobId", jobId);
                            insertCommand.Parameters.AddWithValue("@driverId", driverId);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error assigning driver to job: {ex.Message}");
                }
            }
        }

        public void UnassignDriverFromJob(int driverId, int jobId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE transport_units SET driver_id = NULL, status = 'pending' WHERE driver_id = @driverId AND job_id = @jobId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@jobId", jobId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error unassigning driver from job: {ex.Message}");
                }
            }
        }

        // Statistics and Reporting
        public int GetTotalActiveDrivers()
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM staff WHERE is_active = true AND (position LIKE '%driver%' OR position LIKE '%Driver%')";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting total active drivers: {ex.Message}");
                }
            }
        }

        public int GetTotalDrivers()
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM staff WHERE position LIKE '%driver%' OR position LIKE '%Driver%'";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting total drivers: {ex.Message}");
                }
            }
        }

        public List<Staff> GetDriversHiredInPeriod(DateTime startDate, DateTime endDate)
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE (s.position LIKE '%driver%' OR s.position LIKE '%Driver%') 
                                   AND s.hire_date BETWEEN @startDate AND @endDate
                                   ORDER BY s.hire_date DESC";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(MapStaffFromReader(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving drivers hired in period: {ex.Message}");
                }
            }
            return drivers;
        }

        public decimal GetAverageSalary()
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT AVG(salary) FROM staff WHERE is_active = true AND (position LIKE '%driver%' OR position LIKE '%Driver%')";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error calculating average salary: {ex.Message}");
                }
            }
        }

        public Staff? GetDriverWithHighestSalary()
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.is_active = true AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%')
                                   ORDER BY s.salary DESC LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapStaffFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver with highest salary: {ex.Message}");
                }
            }
            return null;
        }

        public Staff? GetDriverWithLowestSalary()
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.is_active = true AND (s.position LIKE '%driver%' OR s.position LIKE '%Driver%') AND s.salary > 0
                                   ORDER BY s.salary ASC LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapStaffFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver with lowest salary: {ex.Message}");
                }
            }
            return null;
        }

        // Job History and Tracking
        public List<TransportUnit> GetDriverJobHistory(int driverId)
        {
            var transportUnits = new List<TransportUnit>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT tu.*, j.job_number, j.pickup_city, j.destination_city, j.status as job_status,
                                   j.requested_pickup_date, j.requested_delivery_date
                                   FROM transport_units tu 
                                   INNER JOIN jobs j ON tu.job_id = j.job_id 
                                   WHERE tu.driver_id = @driverId
                                   ORDER BY j.requested_pickup_date DESC";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var transportUnit = new TransportUnit
                                {
                                    TransportUnitId = Convert.ToInt32(reader["transport_unit_id"]),
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    VehicleId = reader["vehicle_id"] != DBNull.Value ? Convert.ToInt32(reader["vehicle_id"]) : null,
                                    ContainerId = reader["container_id"] != DBNull.Value ? Convert.ToInt32(reader["container_id"]) : null,
                                    DriverId = reader["driver_id"] != DBNull.Value ? Convert.ToInt32(reader["driver_id"]) : null,
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    Job = new Job
                                    {
                                        JobId = Convert.ToInt32(reader["job_id"]),
                                        JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                        PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                        DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                        Status = reader["job_status"].ToString() ?? string.Empty,
                                        RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                        RequestedDeliveryDate = reader["requested_delivery_date"] != DBNull.Value ? 
                                            Convert.ToDateTime(reader["requested_delivery_date"]) : null
                                    }
                                };
                                transportUnits.Add(transportUnit);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver job history: {ex.Message}");
                }
            }
            return transportUnits;
        }

        public int GetDriverJobCount(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM transport_units WHERE driver_id = @driverId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting driver job count: {ex.Message}");
                }
            }
        }

        public TransportUnit? GetDriverCurrentJob(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT tu.*, j.job_number, j.pickup_city, j.destination_city, j.status as job_status,
                                   j.requested_pickup_date, j.requested_delivery_date
                                   FROM transport_units tu 
                                   INNER JOIN jobs j ON tu.job_id = j.job_id 
                                   WHERE tu.driver_id = @driverId AND tu.status IN ('assigned', 'in_progress')
                                   ORDER BY j.requested_pickup_date ASC LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TransportUnit
                                {
                                    TransportUnitId = Convert.ToInt32(reader["transport_unit_id"]),
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    VehicleId = reader["vehicle_id"] != DBNull.Value ? Convert.ToInt32(reader["vehicle_id"]) : null,
                                    ContainerId = reader["container_id"] != DBNull.Value ? Convert.ToInt32(reader["container_id"]) : null,
                                    DriverId = reader["driver_id"] != DBNull.Value ? Convert.ToInt32(reader["driver_id"]) : null,
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    Job = new Job
                                    {
                                        JobId = Convert.ToInt32(reader["job_id"]),
                                        JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                        PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                        DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                        Status = reader["job_status"].ToString() ?? string.Empty,
                                        RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                        RequestedDeliveryDate = reader["requested_delivery_date"] != DBNull.Value ? 
                                            Convert.ToDateTime(reader["requested_delivery_date"]) : null
                                    }
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving driver current job: {ex.Message}");
                }
            }
            return null;
        }

        // Validation Methods
        public bool IsPhoneNumberUnique(string phone, int? excludeDriverId = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM staff WHERE phone = @phone";
                    if (excludeDriverId.HasValue)
                    {
                        query += " AND staff_id != @excludeDriverId";
                    }
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@phone", phone);
                        if (excludeDriverId.HasValue)
                        {
                            command.Parameters.AddWithValue("@excludeDriverId", excludeDriverId.Value);
                        }
                        return Convert.ToInt32(command.ExecuteScalar()) == 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking phone number uniqueness: {ex.Message}");
                }
            }
        }

        public bool IsUserIdUnique(int userId, int? excludeDriverId = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM staff WHERE user_id = @userId";
                    if (excludeDriverId.HasValue)
                    {
                        query += " AND staff_id != @excludeDriverId";
                    }
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        if (excludeDriverId.HasValue)
                        {
                            command.Parameters.AddWithValue("@excludeDriverId", excludeDriverId.Value);
                        }
                        return Convert.ToInt32(command.ExecuteScalar()) == 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking user ID uniqueness: {ex.Message}");
                }
            }
        }

        // Helper method to map Staff from database reader
        private Staff MapStaffFromReader(MySqlDataReader reader)
        {
            return new Staff
            {
                StaffId = Convert.ToInt32(reader["staff_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                FirstName = reader["first_name"].ToString() ?? string.Empty,
                LastName = reader["last_name"].ToString() ?? string.Empty,
                Phone = reader["phone"].ToString() ?? string.Empty,
                Address = reader["address"].ToString() ?? string.Empty,
                Position = reader["position"].ToString() ?? string.Empty,
                HireDate = Convert.ToDateTime(reader["hire_date"]),
                Salary = Convert.ToDecimal(reader["salary"]),
                IsActive = Convert.ToBoolean(reader["is_active"]),
                User = reader["username"] != DBNull.Value ? new User
                {
                    UserId = Convert.ToInt32(reader["user_id"]),
                    Username = reader["username"].ToString() ?? string.Empty,
                    Email = reader["email"].ToString() ?? string.Empty
                } : null
            };
        }
    }
}