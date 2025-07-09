using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class AssistantRepository
    {
        public Assistant? GetAssistantById(int assistantId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT a.*, s.first_name, s.last_name, s.phone, s.address, s.position, 
                                   s.hire_date, s.salary, s.is_active as staff_active, j.job_number
                                   FROM assistants a 
                                   INNER JOIN staff s ON a.staff_id = s.staff_id 
                                   LEFT JOIN jobs j ON a.current_job_id = j.job_id
                                   WHERE a.assistant_id = @assistantId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@assistantId", assistantId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Assistant
                                {
                                    AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    Specialization = reader["specialization"].ToString(),
                                    IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                    CurrentJobId = reader["current_job_id"] != DBNull.Value ? 
                                        Convert.ToInt32(reader["current_job_id"]) : null,
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
                                    CurrentJob = reader["current_job_id"] != DBNull.Value ? new Job
                                    {
                                        JobId = Convert.ToInt32(reader["current_job_id"]),
                                        JobNumber = reader["job_number"].ToString() ?? string.Empty
                                    } : null
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving assistant: {ex.Message}");
                }
            }
            return null;
        }

        public List<Assistant> GetAllAssistants()
        {
            var assistants = new List<Assistant>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT a.*, s.first_name, s.last_name, s.phone, s.position, s.is_active as staff_active 
                                   FROM assistants a 
                                   INNER JOIN staff s ON a.staff_id = s.staff_id 
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assistants.Add(new Assistant
                            {
                                AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                StaffId = Convert.ToInt32(reader["staff_id"]),
                                Specialization = reader["specialization"].ToString(),
                                IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                CurrentJobId = reader["current_job_id"] != DBNull.Value ? 
                                    Convert.ToInt32(reader["current_job_id"]) : null,
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
                    throw new Exception($"Error retrieving assistants: {ex.Message}");
                }
            }
            return assistants;
        }

        public List<Assistant> GetAvailableAssistants()
        {
            var assistants = new List<Assistant>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT a.*, s.first_name, s.last_name, s.phone, s.position, s.is_active as staff_active 
                                   FROM assistants a 
                                   INNER JOIN staff s ON a.staff_id = s.staff_id 
                                   WHERE a.is_available = TRUE AND s.is_active = TRUE
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assistants.Add(new Assistant
                            {
                                AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                StaffId = Convert.ToInt32(reader["staff_id"]),
                                Specialization = reader["specialization"].ToString(),
                                IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                CurrentJobId = reader["current_job_id"] != DBNull.Value ? 
                                    Convert.ToInt32(reader["current_job_id"]) : null,
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
                    throw new Exception($"Error retrieving available assistants: {ex.Message}");
                }
            }
            return assistants;
        }

        public int AddAssistant(Assistant assistant)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO assistants (staff_id, specialization, is_available, current_job_id, rating) 
                                   VALUES (@staffId, @specialization, @isAvailable, @currentJobId, @rating);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@staffId", assistant.StaffId);
                        command.Parameters.AddWithValue("@specialization", assistant.Specialization ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isAvailable", assistant.IsAvailable);
                        command.Parameters.AddWithValue("@currentJobId", assistant.CurrentJobId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@rating", assistant.Rating);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding assistant: {ex.Message}");
                }
            }
        }

        public void UpdateAssistant(Assistant assistant)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE assistants SET specialization = @specialization, is_available = @isAvailable, 
                                   current_job_id = @currentJobId, rating = @rating, updated_at = CURRENT_TIMESTAMP 
                                   WHERE assistant_id = @assistantId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@specialization", assistant.Specialization ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isAvailable", assistant.IsAvailable);
                        command.Parameters.AddWithValue("@currentJobId", assistant.CurrentJobId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@rating", assistant.Rating);
                        command.Parameters.AddWithValue("@assistantId", assistant.AssistantId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating assistant: {ex.Message}");
                }
            }
        }

        public void DeleteAssistant(int assistantId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM assistants WHERE assistant_id = @assistantId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@assistantId", assistantId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting assistant: {ex.Message}");
                }
            }
        }

        public void UpdateAssistantAvailability(int assistantId, bool isAvailable, int? currentJobId = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE assistants SET is_available = @isAvailable, current_job_id = @currentJobId, 
                                   updated_at = CURRENT_TIMESTAMP WHERE assistant_id = @assistantId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@isAvailable", isAvailable);
                        command.Parameters.AddWithValue("@currentJobId", currentJobId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@assistantId", assistantId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating assistant availability: {ex.Message}");
                }
            }
        }

        public List<Assistant> GetAssistantsBySpecialization(string specialization)
        {
            var assistants = new List<Assistant>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT a.*, s.first_name, s.last_name, s.phone, s.position, s.is_active as staff_active 
                                   FROM assistants a 
                                   INNER JOIN staff s ON a.staff_id = s.staff_id 
                                   WHERE a.specialization = @specialization AND s.is_active = TRUE
                                   ORDER BY s.first_name, s.last_name";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@specialization", specialization);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                assistants.Add(new Assistant
                                {
                                    AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    Specialization = reader["specialization"].ToString(),
                                    IsAvailable = Convert.ToBoolean(reader["is_available"]),
                                    CurrentJobId = reader["current_job_id"] != DBNull.Value ? 
                                        Convert.ToInt32(reader["current_job_id"]) : null,
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
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving assistants by specialization: {ex.Message}");
                }
            }
            return assistants;
        }
    }
}