using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        public Staff? GetStaffById(int staffId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT s.*, u.username, u.email FROM staff s JOIN users u ON s.user_id = u.user_id WHERE s.staff_id = @staffId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@staffId", staffId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Staff
                                {
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Position = reader["position"].ToString() ?? string.Empty,
                                    HireDate = Convert.ToDateTime(reader["hire_date"]),
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
                    throw new Exception($"Error retrieving staff: {ex.Message}");
                }
            }
            return null;
        }

        public List<Staff> GetAllStaff()
        {
            var staffList = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT s.*, u.username, u.email FROM staff s JOIN users u ON s.user_id = u.user_id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                staffList.Add(new Staff
                                {
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Position = reader["position"].ToString() ?? string.Empty,
                                    HireDate = Convert.ToDateTime(reader["hire_date"]),
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
                    throw new Exception($"Error retrieving all staff: {ex.Message}");
                }
            }
            return staffList;
        }

        public List<Staff> GetAllDrivers()
        {
            var drivers = new List<Staff>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT s.*, u.username, u.email FROM staff s JOIN users u ON s.user_id = u.user_id WHERE s.position = 'Lorry Driver'";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                drivers.Add(new Staff
                                {
                                    StaffId = Convert.ToInt32(reader["staff_id"]),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    FirstName = reader["first_name"].ToString() ?? string.Empty,
                                    LastName = reader["last_name"].ToString() ?? string.Empty,
                                    Phone = reader["phone"].ToString() ?? string.Empty,
                                    Position = reader["position"].ToString() ?? string.Empty,
                                    HireDate = Convert.ToDateTime(reader["hire_date"]),
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

        public int AddStaff(Staff staff)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO staff (user_id, first_name, last_name, phone, position, hire_date) 
                                   VALUES (@userId, @firstName, @lastName, @phone, @position, @hireDate);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", staff.UserId);
                        command.Parameters.AddWithValue("@firstName", staff.FirstName);
                        command.Parameters.AddWithValue("@lastName", staff.LastName);
                        command.Parameters.AddWithValue("@phone", staff.Phone);
                        command.Parameters.AddWithValue("@position", staff.Position);
                        command.Parameters.AddWithValue("@hireDate", staff.HireDate);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding staff: {ex.Message}");
                }
            }
        }

        public void UpdateStaff(Staff staff)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE staff SET first_name = @firstName, last_name = @lastName, phone = @phone, 
                                   position = @position, hire_date = @hireDate 
                                   WHERE staff_id = @staffId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", staff.FirstName);
                        command.Parameters.AddWithValue("@lastName", staff.LastName);
                        command.Parameters.AddWithValue("@phone", staff.Phone);
                        command.Parameters.AddWithValue("@position", staff.Position);
                        command.Parameters.AddWithValue("@hireDate", staff.HireDate);
                        command.Parameters.AddWithValue("@staffId", staff.StaffId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating staff: {ex.Message}");
                }
            }
        }

        public void DeleteStaff(int staffId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM staff WHERE staff_id = @staffId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@staffId", staffId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting staff: {ex.Message}");
                }
            }
        }
    }
}