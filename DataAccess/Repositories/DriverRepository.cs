using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        #region Async Methods

        // Basic CRUD Operations
        public async Task<Staff?> GetDriverByIdAsync(int driverId)
        {
            return await Task.FromResult(GetDriverById(driverId));
        }

        public async Task<Staff?> GetDriverByUserIdAsync(int userId)
        {
            return await Task.FromResult(GetDriverByUserId(userId));
        }

        public async Task<List<Staff>> GetAllDriversAsync()
        {
            return await Task.FromResult(GetAllDrivers());
        }

        public async Task<int> CreateDriverAsync(Staff driver)
        {
            return await Task.FromResult(CreateDriver(driver));
        }

        public async Task UpdateDriverAsync(Staff driver)
        {
            await Task.Run(() => UpdateDriver(driver));
        }

        public async Task DeleteDriverAsync(int driverId)
        {
            await Task.Run(() => DeleteDriver(driverId));
        }

        public async Task<bool> DriverExistsAsync(int driverId)
        {
            return await Task.FromResult(DriverExists(driverId));
        }

        // Driver-Specific Queries
        public async Task<List<Staff>> GetActiveDriversAsync()
        {
            return await Task.FromResult(GetActiveDrivers());
        }

        public async Task<List<Staff>> GetAvailableDriversAsync()
        {
            return await Task.FromResult(GetAvailableDrivers());
        }

        public async Task<List<Staff>> GetDriversByPositionAsync(string position)
        {
            return await Task.FromResult(GetDriversByPosition(position));
        }

        public async Task<Staff?> GetDriverByPhoneAsync(string phone)
        {
            return await Task.FromResult(GetDriverByPhone(phone));
        }

        public async Task<List<Staff>> SearchDriversAsync(string searchTerm)
        {
            return await Task.FromResult(SearchDrivers(searchTerm));
        }

        // Assignment and Availability Management
        public async Task<bool> IsDriverAvailableAsync(int driverId, DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(IsDriverAvailable(driverId, startDate, endDate));
        }

        public async Task<List<Staff>> GetDriversAvailableForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(GetDriversAvailableForDateRange(startDate, endDate));
        }

        public async Task AssignDriverToJobAsync(int driverId, int transportUnitId)
        {
            await Task.Run(() => AssignDriverToJob(driverId, transportUnitId));
        }

        public async Task UnassignDriverFromJobAsync(int driverId, int transportUnitId)
        {
            await Task.Run(() => UnassignDriverFromJob(driverId, transportUnitId));
        }

        // Statistics and Reporting
        public async Task<int> GetTotalActiveDriversAsync()
        {
            return await Task.FromResult(GetTotalActiveDrivers());
        }

        public async Task<int> GetTotalDriversAsync()
        {
            return await Task.FromResult(GetTotalDrivers());
        }

        public async Task<List<Staff>> GetDriversHiredInPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(GetDriversHiredInPeriod(startDate, endDate));
        }

        public async Task<decimal> GetAverageSalaryAsync()
        {
            return await Task.FromResult(GetAverageSalary());
        }

        public async Task<Staff?> GetDriverWithHighestSalaryAsync()
        {
            return await Task.FromResult(GetDriverWithHighestSalary());
        }

        public async Task<Staff?> GetDriverWithLowestSalaryAsync()
        {
            return await Task.FromResult(GetDriverWithLowestSalary());
        }

        // Job History and Tracking
        public async Task<List<TransportUnit>> GetDriverJobHistoryAsync(int driverId)
        {
            return await Task.FromResult(GetDriverJobHistory(driverId));
        }

        public async Task<int> GetDriverJobCountAsync(int driverId)
        {
            return await Task.FromResult(GetDriverJobCount(driverId));
        }

        public async Task<TransportUnit?> GetDriverCurrentJobAsync(int driverId)
        {
            return await Task.FromResult(GetDriverCurrentJob(driverId));
        }

        // Validation Methods
        public async Task<bool> IsPhoneNumberUniqueAsync(string phone, int? excludeDriverId = null)
        {
            return await Task.FromResult(IsPhoneNumberUnique(phone, excludeDriverId));
        }

        public async Task<bool> IsUserIdUniqueAsync(int userId, int? excludeDriverId = null)
        {
            return await Task.FromResult(IsUserIdUnique(userId, excludeDriverId));
        }

        #endregion

        #region Synchronous Methods

        // Basic CRUD Operations
        public Staff? GetDriverById(int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.staff_id = @driverId AND s.position LIKE '%driver%'";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateStaffFromReader(reader);
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

        public Staff? GetDriverByUserId(int userId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.user_id = @userId AND s.position LIKE '%driver%'";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateStaffFromReader(reader);
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
                                   WHERE s.position LIKE '%driver%' 
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(CreateStaffFromReader(reader));
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
                        command.Parameters.AddWithValue("@position", driver.Position.Contains("driver", StringComparison.OrdinalIgnoreCase) ? driver.Position : "Driver");
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
                                   salary = @salary, is_active = @isActive 
                                   WHERE staff_id = @staffId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", driver.UserId);
                        command.Parameters.AddWithValue("@firstName", driver.FirstName);
                        command.Parameters.AddWithValue("@lastName", driver.LastName);
                        command.Parameters.AddWithValue("@phone", driver.Phone);
                        command.Parameters.AddWithValue("@address", driver.Address);
                        command.Parameters.AddWithValue("@position", driver.Position.Contains("driver", StringComparison.OrdinalIgnoreCase) ? driver.Position : "Driver");
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
                    
                    // First check if driver is assigned to any active transport units
                    string checkQuery = @"SELECT COUNT(*) FROM transport_units 
                                        WHERE driver_id = @driverId AND status IN ('pending', 'in_progress', 'assigned')";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@driverId", driverId);
                        int activeAssignments = Convert.ToInt32(checkCommand.ExecuteScalar());
                        
                        if (activeAssignments > 0)
                        {
                            throw new Exception("Cannot delete driver with active job assignments. Please complete or reassign jobs first.");
                        }
                    }

                    // Soft delete by setting is_active to false
                    string query = @"UPDATE staff SET is_active = 0 WHERE staff_id = @driverId AND position LIKE '%driver%'";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        int affected = command.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            throw new Exception("Driver not found or already deleted.");
                        }
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
                    string query = @"SELECT COUNT(*) FROM staff 
                                   WHERE staff_id = @driverId AND position LIKE '%driver%'";
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
                                   WHERE s.position LIKE '%driver%' AND s.is_active = 1 
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(CreateStaffFromReader(reader));
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
                                   WHERE s.position LIKE '%driver%' AND s.is_active = 1 
                                   AND s.staff_id NOT IN (
                                       SELECT DISTINCT driver_id FROM transport_units 
                                       WHERE driver_id IS NOT NULL 
                                       AND status IN ('pending', 'in_progress', 'assigned')
                                   )
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(CreateStaffFromReader(reader));
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
                                   WHERE s.position LIKE @position AND s.position LIKE '%driver%' 
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@position", $"%{position}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(CreateStaffFromReader(reader));
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
                                   WHERE s.phone = @phone AND s.position LIKE '%driver%'";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@phone", phone);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateStaffFromReader(reader);
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
                                   WHERE s.position LIKE '%driver%' 
                                   AND (s.first_name LIKE @searchTerm OR s.last_name LIKE @searchTerm 
                                        OR s.phone LIKE @searchTerm OR s.address LIKE @searchTerm
                                        OR CONCAT(s.first_name, ' ', s.last_name) LIKE @searchTerm)
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(CreateStaffFromReader(reader));
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
                                   WHERE tu.driver_id = @driverId 
                                   AND tu.status IN ('pending', 'in_progress', 'assigned')
                                   AND (
                                       (j.requested_pickup_date BETWEEN @startDate AND @endDate) OR
                                       (j.requested_delivery_date BETWEEN @startDate AND @endDate) OR
                                       (j.requested_pickup_date <= @startDate AND j.requested_delivery_date >= @endDate)
                                   )";
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
                    string query = @"SELECT s.*, u.username, u.email FROM staff s 
                                   LEFT JOIN users u ON s.user_id = u.user_id 
                                   WHERE s.position LIKE '%driver%' AND s.is_active = 1 
                                   AND s.staff_id NOT IN (
                                       SELECT DISTINCT tu.driver_id FROM transport_units tu 
                                       INNER JOIN jobs j ON tu.job_id = j.job_id 
                                       WHERE tu.driver_id IS NOT NULL 
                                       AND tu.status IN ('pending', 'in_progress', 'assigned')
                                       AND (
                                           (j.requested_pickup_date BETWEEN @startDate AND @endDate) OR
                                           (j.requested_delivery_date BETWEEN @startDate AND @endDate) OR
                                           (j.requested_pickup_date <= @startDate AND j.requested_delivery_date >= @endDate)
                                       )
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
                                drivers.Add(CreateStaffFromReader(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving available drivers for date range: {ex.Message}");
                }
            }
            return drivers;
        }

        public void AssignDriverToJob(int driverId, int transportUnitId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    
                    // First verify the driver exists and is active
                    if (!DriverExists(driverId))
                    {
                        throw new Exception("Driver not found.");
                    }

                    string query = @"UPDATE transport_units SET driver_id = @driverId, status = 'assigned' 
                                   WHERE transport_unit_id = @transportUnitId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@transportUnitId", transportUnitId);
                        int affected = command.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            throw new Exception("Transport unit not found or already assigned.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error assigning driver to job: {ex.Message}");
                }
            }
        }

        public void UnassignDriverFromJob(int driverId, int transportUnitId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE transport_units SET driver_id = NULL, status = 'pending' 
                                   WHERE transport_unit_id = @transportUnitId AND driver_id = @driverId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        command.Parameters.AddWithValue("@transportUnitId", transportUnitId);
                        int affected = command.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            throw new Exception("Assignment not found or driver not assigned to this transport unit.");
                        }
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
                    string query = @"SELECT COUNT(*) FROM staff 
                                   WHERE position LIKE '%driver%' AND is_active = 1";
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
                    string query = @"SELECT COUNT(*) FROM staff WHERE position LIKE '%driver%'";
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
                                   WHERE s.position LIKE '%driver%' 
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
                                drivers.Add(CreateStaffFromReader(reader));
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
                    string query = @"SELECT AVG(salary) FROM staff 
                                   WHERE position LIKE '%driver%' AND is_active = 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting average salary: {ex.Message}");
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
                                   WHERE s.position LIKE '%driver%' AND s.is_active = 1 
                                   ORDER BY s.salary DESC LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateStaffFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting driver with highest salary: {ex.Message}");
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
                                   WHERE s.position LIKE '%driver%' AND s.is_active = 1 
                                   ORDER BY s.salary ASC LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateStaffFromReader(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting driver with lowest salary: {ex.Message}");
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
                                           v.registration_number, v.make, v.model,
                                           s.first_name, s.last_name
                                   FROM transport_units tu 
                                   INNER JOIN jobs j ON tu.job_id = j.job_id
                                   LEFT JOIN vehicles v ON tu.vehicle_id = v.vehicle_id
                                   LEFT JOIN staff s ON tu.driver_id = s.staff_id
                                   WHERE tu.driver_id = @driverId 
                                   ORDER BY j.created_at DESC";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transportUnits.Add(CreateTransportUnitFromReader(reader));
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
                    string query = @"SELECT COUNT(*) FROM transport_units 
                                   WHERE driver_id = @driverId";
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
                                           v.registration_number, v.make, v.model,
                                           s.first_name, s.last_name
                                   FROM transport_units tu 
                                   INNER JOIN jobs j ON tu.job_id = j.job_id
                                   LEFT JOIN vehicles v ON tu.vehicle_id = v.vehicle_id
                                   LEFT JOIN staff s ON tu.driver_id = s.staff_id
                                   WHERE tu.driver_id = @driverId 
                                   AND tu.status IN ('pending', 'in_progress', 'assigned')
                                   ORDER BY j.created_at DESC LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateTransportUnitFromReader(reader);
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
                    string query = @"SELECT COUNT(*) FROM staff 
                                   WHERE phone = @phone AND position LIKE '%driver%'";
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
                    string query = @"SELECT COUNT(*) FROM staff 
                                   WHERE user_id = @userId AND position LIKE '%driver%'";
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

        #endregion

        #region Helper Methods

        private Staff CreateStaffFromReader(MySqlDataReader reader)
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

        private TransportUnit CreateTransportUnitFromReader(MySqlDataReader reader)
        {
            return new TransportUnit
            {
                TransportUnitId = Convert.ToInt32(reader["transport_unit_id"]),
                JobId = Convert.ToInt32(reader["job_id"]),
                VehicleId = reader["vehicle_id"] == DBNull.Value ? null : Convert.ToInt32(reader["vehicle_id"]),
                ContainerId = reader["container_id"] == DBNull.Value ? null : Convert.ToInt32(reader["container_id"]),
                DriverId = reader["driver_id"] == DBNull.Value ? null : Convert.ToInt32(reader["driver_id"]),
                Status = reader["status"].ToString() ?? string.Empty,
                Job = new Job
                {
                    JobId = Convert.ToInt32(reader["job_id"]),
                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                    Status = reader["job_status"].ToString() ?? string.Empty
                },
                Vehicle = reader["registration_number"] != DBNull.Value ? new Vehicle
                {
                    VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                    RegistrationNumber = reader["registration_number"].ToString() ?? string.Empty,
                    Make = reader["make"].ToString() ?? string.Empty,
                    Model = reader["model"].ToString() ?? string.Empty
                } : null,
                Driver = reader["first_name"] != DBNull.Value ? new Staff
                {
                    StaffId = Convert.ToInt32(reader["driver_id"]),
                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                    LastName = reader["last_name"].ToString() ?? string.Empty
                } : null
            };
        }

        #endregion
    }
}