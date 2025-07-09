using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class AuditLogRepository
    {
        public void LogAction(string tableName, int recordId, string actionType, 
            string? oldValues, string? newValues, int? changedBy, string? ipAddress = null, string? userAgent = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO audit_logs (table_name, record_id, action_type, old_values, 
                                   new_values, changed_by, ip_address, user_agent) 
                                   VALUES (@tableName, @recordId, @actionType, @oldValues, @newValues, 
                                   @changedBy, @ipAddress, @userAgent)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tableName", tableName);
                        command.Parameters.AddWithValue("@recordId", recordId);
                        command.Parameters.AddWithValue("@actionType", actionType);
                        command.Parameters.AddWithValue("@oldValues", oldValues ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@newValues", newValues ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@changedBy", changedBy ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ipAddress", ipAddress ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@userAgent", userAgent ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Don't throw exception for audit logging failures to avoid breaking main operations
                    Console.WriteLine($"Audit logging failed: {ex.Message}");
                }
            }
        }

        public List<AuditLog> GetAuditLogs(int pageNumber = 1, int pageSize = 50, string? tableName = null, 
            string? actionType = null, int? changedBy = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var auditLogs = new List<AuditLog>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    
                    var whereConditions = new List<string>();
                    if (!string.IsNullOrEmpty(tableName))
                        whereConditions.Add("al.table_name = @tableName");
                    if (!string.IsNullOrEmpty(actionType))
                        whereConditions.Add("al.action_type = @actionType");
                    if (changedBy.HasValue)
                        whereConditions.Add("al.changed_by = @changedBy");
                    if (fromDate.HasValue)
                        whereConditions.Add("al.created_at >= @fromDate");
                    if (toDate.HasValue)
                        whereConditions.Add("al.created_at <= @toDate");

                    string whereClause = whereConditions.Count > 0 ? " WHERE " + string.Join(" AND ", whereConditions) : "";
                    
                    string query = $@"SELECT al.*, u.username, u.email 
                                    FROM audit_logs al 
                                    LEFT JOIN users u ON al.changed_by = u.user_id 
                                    {whereClause}
                                    ORDER BY al.created_at DESC 
                                    LIMIT @offset, @pageSize";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(tableName))
                            command.Parameters.AddWithValue("@tableName", tableName);
                        if (!string.IsNullOrEmpty(actionType))
                            command.Parameters.AddWithValue("@actionType", actionType);
                        if (changedBy.HasValue)
                            command.Parameters.AddWithValue("@changedBy", changedBy);
                        if (fromDate.HasValue)
                            command.Parameters.AddWithValue("@fromDate", fromDate);
                        if (toDate.HasValue)
                            command.Parameters.AddWithValue("@toDate", toDate);
                        
                        command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                        command.Parameters.AddWithValue("@pageSize", pageSize);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                auditLogs.Add(new AuditLog
                                {
                                    AuditId = Convert.ToInt32(reader["audit_id"]),
                                    TableName = reader["table_name"].ToString() ?? string.Empty,
                                    RecordId = Convert.ToInt32(reader["record_id"]),
                                    ActionType = reader["action_type"].ToString() ?? string.Empty,
                                    OldValues = reader["old_values"].ToString(),
                                    NewValues = reader["new_values"].ToString(),
                                    ChangedBy = reader["changed_by"] != DBNull.Value ? Convert.ToInt32(reader["changed_by"]) : null,
                                    IpAddress = reader["ip_address"].ToString(),
                                    UserAgent = reader["user_agent"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    ChangedByUser = reader["changed_by"] != DBNull.Value ? new User
                                    {
                                        UserId = Convert.ToInt32(reader["changed_by"]),
                                        Username = reader["username"].ToString() ?? string.Empty,
                                        Email = reader["email"].ToString() ?? string.Empty
                                    } : null
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving audit logs: {ex.Message}");
                }
            }
            return auditLogs;
        }

        public int GetAuditLogsCount(string? tableName = null, string? actionType = null, 
            int? changedBy = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    
                    var whereConditions = new List<string>();
                    if (!string.IsNullOrEmpty(tableName))
                        whereConditions.Add("table_name = @tableName");
                    if (!string.IsNullOrEmpty(actionType))
                        whereConditions.Add("action_type = @actionType");
                    if (changedBy.HasValue)
                        whereConditions.Add("changed_by = @changedBy");
                    if (fromDate.HasValue)
                        whereConditions.Add("created_at >= @fromDate");
                    if (toDate.HasValue)
                        whereConditions.Add("created_at <= @toDate");

                    string whereClause = whereConditions.Count > 0 ? " WHERE " + string.Join(" AND ", whereConditions) : "";
                    
                    string query = $"SELECT COUNT(*) FROM audit_logs {whereClause}";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(tableName))
                            command.Parameters.AddWithValue("@tableName", tableName);
                        if (!string.IsNullOrEmpty(actionType))
                            command.Parameters.AddWithValue("@actionType", actionType);
                        if (changedBy.HasValue)
                            command.Parameters.AddWithValue("@changedBy", changedBy);
                        if (fromDate.HasValue)
                            command.Parameters.AddWithValue("@fromDate", fromDate);
                        if (toDate.HasValue)
                            command.Parameters.AddWithValue("@toDate", toDate);
                        
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error getting audit logs count: {ex.Message}");
                }
            }
        }

        public List<AuditLog> GetAuditLogsForRecord(string tableName, int recordId)
        {
            var auditLogs = new List<AuditLog>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT al.*, u.username, u.email 
                                   FROM audit_logs al 
                                   LEFT JOIN users u ON al.changed_by = u.user_id 
                                   WHERE al.table_name = @tableName AND al.record_id = @recordId 
                                   ORDER BY al.created_at DESC";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tableName", tableName);
                        command.Parameters.AddWithValue("@recordId", recordId);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                auditLogs.Add(new AuditLog
                                {
                                    AuditId = Convert.ToInt32(reader["audit_id"]),
                                    TableName = reader["table_name"].ToString() ?? string.Empty,
                                    RecordId = Convert.ToInt32(reader["record_id"]),
                                    ActionType = reader["action_type"].ToString() ?? string.Empty,
                                    OldValues = reader["old_values"].ToString(),
                                    NewValues = reader["new_values"].ToString(),
                                    ChangedBy = reader["changed_by"] != DBNull.Value ? Convert.ToInt32(reader["changed_by"]) : null,
                                    IpAddress = reader["ip_address"].ToString(),
                                    UserAgent = reader["user_agent"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    ChangedByUser = reader["changed_by"] != DBNull.Value ? new User
                                    {
                                        UserId = Convert.ToInt32(reader["changed_by"]),
                                        Username = reader["username"].ToString() ?? string.Empty,
                                        Email = reader["email"].ToString() ?? string.Empty
                                    } : null
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving audit logs for record: {ex.Message}");
                }
            }
            return auditLogs;
        }

        public List<string> GetAuditedTables()
        {
            var tables = new List<string>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT DISTINCT table_name FROM audit_logs ORDER BY table_name";
                    
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["table_name"].ToString() ?? string.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving audited tables: {ex.Message}");
                }
            }
            return tables;
        }

        public void CleanupOldAuditLogs(int daysToKeep = 365)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM audit_logs WHERE created_at < DATE_SUB(NOW(), INTERVAL @daysToKeep DAY)";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@daysToKeep", daysToKeep);
                        int deletedRows = command.ExecuteNonQuery();
                        Console.WriteLine($"Cleaned up {deletedRows} audit log records older than {daysToKeep} days");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error cleaning up audit logs: {ex.Message}");
                }
            }
        }
    }
}