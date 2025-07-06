using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class JobRepository : IJobRepository
    {
        public Job? GetJobById(int jobId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   WHERE j.job_id = @jobId";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jobId", jobId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    PickupPostalCode = reader["pickup_postal_code"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    DestinationPostalCode = reader["destination_postal_code"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    RequestedDeliveryDate = reader["requested_delivery_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["requested_delivery_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    TotalEstimatedWeight = reader["total_estimated_weight"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_weight"]),
                                    TotalEstimatedVolume = reader["total_estimated_volume"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_volume"]),
                                    SpecialInstructions = reader["special_instructions"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        User = new User
                                        {
                                            Email = reader["email"].ToString() ?? string.Empty
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving job: {ex.Message}");
                }
            }
            return null;
        }

        public List<Job> GetAllJobs()
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   ORDER BY j.created_at DESC";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    PickupPostalCode = reader["pickup_postal_code"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    DestinationPostalCode = reader["destination_postal_code"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    RequestedDeliveryDate = reader["requested_delivery_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["requested_delivery_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    TotalEstimatedWeight = reader["total_estimated_weight"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_weight"]),
                                    TotalEstimatedVolume = reader["total_estimated_volume"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_volume"]),
                                    SpecialInstructions = reader["special_instructions"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        User = new User
                                        {
                                            Email = reader["email"].ToString() ?? string.Empty
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving jobs: {ex.Message}");
                }
            }
            return jobs;
        }

        public List<Job> GetJobsByCustomerId(int customerId)
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   WHERE j.customer_id = @customerId 
                                   ORDER BY j.created_at DESC";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    PickupPostalCode = reader["pickup_postal_code"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    DestinationPostalCode = reader["destination_postal_code"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    RequestedDeliveryDate = reader["requested_delivery_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["requested_delivery_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    TotalEstimatedWeight = reader["total_estimated_weight"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_weight"]),
                                    TotalEstimatedVolume = reader["total_estimated_volume"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_volume"]),
                                    SpecialInstructions = reader["special_instructions"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        User = new User
                                        {
                                            Email = reader["email"].ToString() ?? string.Empty
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving jobs for customer: {ex.Message}");
                }
            }
            return jobs;
        }

        public List<Job> GetJobsByStatus(string status)
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   WHERE j.status = @status 
                                   ORDER BY j.created_at DESC";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    PickupPostalCode = reader["pickup_postal_code"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    DestinationPostalCode = reader["destination_postal_code"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    RequestedDeliveryDate = reader["requested_delivery_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["requested_delivery_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    TotalEstimatedWeight = reader["total_estimated_weight"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_weight"]),
                                    TotalEstimatedVolume = reader["total_estimated_volume"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_volume"]),
                                    SpecialInstructions = reader["special_instructions"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        User = new User
                                        {
                                            Email = reader["email"].ToString() ?? string.Empty
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving jobs by status: {ex.Message}");
                }
            }
            return jobs;
        }

        public List<Job> GetJobsByDateRange(DateTime startDate, DateTime endDate)
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   WHERE j.created_at BETWEEN @startDate AND @endDate 
                                   ORDER BY j.created_at DESC";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    PickupPostalCode = reader["pickup_postal_code"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    DestinationPostalCode = reader["destination_postal_code"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    RequestedDeliveryDate = reader["requested_delivery_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["requested_delivery_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    TotalEstimatedWeight = reader["total_estimated_weight"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_weight"]),
                                    TotalEstimatedVolume = reader["total_estimated_volume"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_volume"]),
                                    SpecialInstructions = reader["special_instructions"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        User = new User
                                        {
                                            Email = reader["email"].ToString() ?? string.Empty
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving jobs by date range: {ex.Message}");
                }
            }
            return jobs;
        }

        public List<Job> SearchJobs(string searchTerm)
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   WHERE j.job_number LIKE @searchTerm 
                                   OR j.pickup_city LIKE @searchTerm 
                                   OR j.destination_city LIKE @searchTerm 
                                   OR CONCAT(c.first_name, ' ', c.last_name) LIKE @searchTerm
                                   ORDER BY j.created_at DESC";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    PickupPostalCode = reader["pickup_postal_code"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    DestinationPostalCode = reader["destination_postal_code"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    RequestedDeliveryDate = reader["requested_delivery_date"] == DBNull.Value ? null : Convert.ToDateTime(reader["requested_delivery_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    TotalEstimatedWeight = reader["total_estimated_weight"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_weight"]),
                                    TotalEstimatedVolume = reader["total_estimated_volume"] == DBNull.Value ? null : Convert.ToDecimal(reader["total_estimated_volume"]),
                                    SpecialInstructions = reader["special_instructions"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty,
                                        Phone = reader["phone"].ToString() ?? string.Empty,
                                        User = new User
                                        {
                                            Email = reader["email"].ToString() ?? string.Empty
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error searching jobs: {ex.Message}");
                }
            }
            return jobs;
        }

        public int AddJob(Job job)
        {
            ArgumentNullException.ThrowIfNull(job);
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO jobs (customer_id, job_number, pickup_address, pickup_city, 
                                   pickup_postal_code, destination_address, destination_city, destination_postal_code, 
                                   requested_pickup_date, requested_delivery_date, status, total_estimated_weight, 
                                   total_estimated_volume, special_instructions, created_at, updated_at) 
                                   VALUES (@customerId, @jobNumber, @pickupAddress, @pickupCity, @pickupPostalCode, 
                                   @destinationAddress, @destinationCity, @destinationPostalCode, @requestedPickupDate, 
                                   @requestedDeliveryDate, @status, @totalEstimatedWeight, @totalEstimatedVolume, 
                                   @specialInstructions, @createdAt, @updatedAt);
                                   SELECT LAST_INSERT_ID();";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", job.CustomerId);
                        command.Parameters.AddWithValue("@jobNumber", job.JobNumber);
                        command.Parameters.AddWithValue("@pickupAddress", job.PickupAddress);
                        command.Parameters.AddWithValue("@pickupCity", job.PickupCity);
                        command.Parameters.AddWithValue("@pickupPostalCode", job.PickupPostalCode);
                        command.Parameters.AddWithValue("@destinationAddress", job.DestinationAddress);
                        command.Parameters.AddWithValue("@destinationCity", job.DestinationCity);
                        command.Parameters.AddWithValue("@destinationPostalCode", job.DestinationPostalCode);
                        command.Parameters.AddWithValue("@requestedPickupDate", job.RequestedPickupDate);
                        command.Parameters.AddWithValue("@requestedDeliveryDate", job.RequestedDeliveryDate);
                        command.Parameters.AddWithValue("@status", job.Status);
                        command.Parameters.AddWithValue("@totalEstimatedWeight", job.TotalEstimatedWeight);
                        command.Parameters.AddWithValue("@totalEstimatedVolume", job.TotalEstimatedVolume);
                        command.Parameters.AddWithValue("@specialInstructions", job.SpecialInstructions);
                        command.Parameters.AddWithValue("@createdAt", job.CreatedAt);
                        command.Parameters.AddWithValue("@updatedAt", job.UpdatedAt);
                        
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding job: {ex.Message}");
                }
            }
        }

        public void UpdateJob(Job job)
        {
            ArgumentNullException.ThrowIfNull(job);
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE jobs SET customer_id = @customerId, job_number = @jobNumber, 
                                   pickup_address = @pickupAddress, pickup_city = @pickupCity, 
                                   pickup_postal_code = @pickupPostalCode, destination_address = @destinationAddress, 
                                   destination_city = @destinationCity, destination_postal_code = @destinationPostalCode, 
                                   requested_pickup_date = @requestedPickupDate, requested_delivery_date = @requestedDeliveryDate, 
                                   status = @status, total_estimated_weight = @totalEstimatedWeight, 
                                   total_estimated_volume = @totalEstimatedVolume, special_instructions = @specialInstructions, 
                                   updated_at = @updatedAt 
                                   WHERE job_id = @jobId";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", job.CustomerId);
                        command.Parameters.AddWithValue("@jobNumber", job.JobNumber);
                        command.Parameters.AddWithValue("@pickupAddress", job.PickupAddress);
                        command.Parameters.AddWithValue("@pickupCity", job.PickupCity);
                        command.Parameters.AddWithValue("@pickupPostalCode", job.PickupPostalCode);
                        command.Parameters.AddWithValue("@destinationAddress", job.DestinationAddress);
                        command.Parameters.AddWithValue("@destinationCity", job.DestinationCity);
                        command.Parameters.AddWithValue("@destinationPostalCode", job.DestinationPostalCode);
                        command.Parameters.AddWithValue("@requestedPickupDate", job.RequestedPickupDate);
                        command.Parameters.AddWithValue("@requestedDeliveryDate", job.RequestedDeliveryDate);
                        command.Parameters.AddWithValue("@status", job.Status);
                        command.Parameters.AddWithValue("@totalEstimatedWeight", job.TotalEstimatedWeight);
                        command.Parameters.AddWithValue("@totalEstimatedVolume", job.TotalEstimatedVolume);
                        command.Parameters.AddWithValue("@specialInstructions", job.SpecialInstructions);
                        command.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@jobId", job.JobId);
                        
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating job: {ex.Message}");
                }
            }
        }

        public void UpdateJobStatus(int jobId, string status)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE jobs SET status = @status, updated_at = @updatedAt WHERE job_id = @jobId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@jobId", jobId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating job status: {ex.Message}");
                }
            }
        }

        public void DeleteJob(int jobId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM jobs WHERE job_id = @jobId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jobId", jobId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting job: {ex.Message}");
                }
            }
        }

        public bool JobExists(string jobNumber)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM jobs WHERE job_number = @jobNumber";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jobNumber", jobNumber);
                        var count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking job existence: {ex.Message}");
                }
            }
        }

        public int GetJobCountByStatus(string status)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM jobs WHERE status = @status";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting job count by status: {ex.Message}");
                }
            }
        }

        public List<Job> GetRecentJobs(int count)
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   ORDER BY j.created_at DESC 
                                   LIMIT @count";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@count", count);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving recent jobs: {ex.Message}");
                }
            }
            return jobs;
        }

        public List<Job> GetJobsForDriver(int driverId)
        {
            var jobs = new List<Job>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT j.*, c.first_name, c.last_name, c.phone, u.email 
                                   FROM jobs j 
                                   LEFT JOIN customers c ON j.customer_id = c.customer_id 
                                   LEFT JOIN users u ON c.user_id = u.user_id 
                                   LEFT JOIN transport_units tu ON j.job_id = tu.job_id 
                                   WHERE tu.driver_id = @driverId 
                                   ORDER BY j.requested_pickup_date";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverId", driverId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    PickupAddress = reader["pickup_address"].ToString() ?? string.Empty,
                                    PickupCity = reader["pickup_city"].ToString() ?? string.Empty,
                                    DestinationAddress = reader["destination_address"].ToString() ?? string.Empty,
                                    DestinationCity = reader["destination_city"].ToString() ?? string.Empty,
                                    RequestedPickupDate = Convert.ToDateTime(reader["requested_pickup_date"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving jobs for driver: {ex.Message}");
                }
            }
            return jobs;
        }

        public List<Job> GetJobsByDriverId(int driverId)
        {
            return GetJobsForDriver(driverId);
        }

        public void AssignJobToDriver(int jobId, int driverId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    
                    // Check if transport unit exists for this job
                    string checkQuery = "SELECT transport_unit_id FROM transport_units WHERE job_id = @jobId";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@jobId", jobId);
                        var existingId = checkCommand.ExecuteScalar();
                        
                        if (existingId != null)
                        {
                            // Update existing transport unit
                            string updateQuery = "UPDATE transport_units SET driver_id = @driverId WHERE job_id = @jobId";
                            using (var updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@driverId", driverId);
                                updateCommand.Parameters.AddWithValue("@jobId", jobId);
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
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error assigning job to driver: {ex.Message}");
                }
            }
        }

        public void UpdateJobProgress(int jobId, string progress)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE jobs SET status = @progress, updated_at = @updatedAt WHERE job_id = @jobId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@progress", progress);
                        command.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@jobId", jobId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating job progress: {ex.Message}");
                }
            }
        }
    }
}