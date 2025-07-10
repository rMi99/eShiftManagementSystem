using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        public void AddLog(AuditLog log)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO audit_log (user_id, action, table_affected, record_id, old_values, new_values, ip_address) 
                                   VALUES (@userId, @action, @tableAffected, @recordId, @oldValues, @newValues, @ipAddress)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", log.UserId);
                        command.Parameters.AddWithValue("@action", log.Action);
                        command.Parameters.AddWithValue("@tableAffected", log.TableAffected);
                        command.Parameters.AddWithValue("@recordId", log.RecordId);
                        command.Parameters.AddWithValue("@oldValues", log.OldValues);
                        command.Parameters.AddWithValue("@newValues", log.NewValues);
                        command.Parameters.AddWithValue("@ipAddress", log.IpAddress);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                }
            }
        }

        public List<AuditLog> GetAllLogs()
        {
            var logs = new List<AuditLog>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT a.*, u.username FROM audit_log a LEFT JOIN users u ON a.user_id = u.user_id ORDER BY a.action_timestamp DESC";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new AuditLog
                                {
                                    LogId = Convert.ToInt32(reader["log_id"]),
                                    UserId = reader["user_id"] as int?,
                                    Action = reader["action"].ToString(),
                                    TableAffected = reader["table_affected"].ToString(),
                                    RecordId = reader["record_id"] as int?,
                                    OldValues = reader["old_values"].ToString(),
                                    NewValues = reader["new_values"].ToString(),
                                    ActionTimestamp = Convert.ToDateTime(reader["action_timestamp"]),
                                    IpAddress = reader["ip_address"].ToString(),
                                    User = new User { Username = reader["username"].ToString() }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                }
            }
            return logs;
        }

        public List<AuditLog> GetLogsByCriteria(DateTime? startDate, DateTime? endDate, int? userId, string actionType)
        {
            var logs = new List<AuditLog>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT a.*, u.username FROM audit_log a LEFT JOIN users u ON a.user_id = u.user_id WHERE 1=1";
                    if (startDate.HasValue) query += " AND a.action_timestamp >= @startDate";
                    if (endDate.HasValue) query += " AND a.action_timestamp <= @endDate";
                    if (userId.HasValue) query += " AND a.user_id = @userId";
                    if (!string.IsNullOrEmpty(actionType)) query += " AND a.action = @actionType";
                    query += " ORDER BY a.action_timestamp DESC";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (startDate.HasValue) command.Parameters.AddWithValue("@startDate", startDate.Value);
                        if (endDate.HasValue) command.Parameters.AddWithValue("@endDate", endDate.Value);
                        if (userId.HasValue) command.Parameters.AddWithValue("@userId", userId.Value);
                        if (!string.IsNullOrEmpty(actionType)) command.Parameters.AddWithValue("@actionType", actionType);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new AuditLog
                                {
                                    LogId = Convert.ToInt32(reader["log_id"]),
                                    UserId = reader["user_id"] as int?,
                                    Action = reader["action"].ToString(),
                                    TableAffected = reader["table_affected"].ToString(),
                                    RecordId = reader["record_id"] as int?,
                                    OldValues = reader["old_values"].ToString(),
                                    NewValues = reader["new_values"].ToString(),
                                    ActionTimestamp = Convert.ToDateTime(reader["action_timestamp"]),
                                    IpAddress = reader["ip_address"].ToString(),
                                    User = new User { Username = reader["username"].ToString() }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                   MessageBox.Show("An error occurred while retrieving logs: " + ex.Message);
                   // Handle or log the exception
                }
            }
            return logs;
        }
    }
}