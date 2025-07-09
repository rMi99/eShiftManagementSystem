using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace eShiftManagementSystem.Business.Services
{
    public class AuditService
    {
        private readonly AuditLogRepository _auditLogRepository;

        public AuditService()
        {
            _auditLogRepository = new AuditLogRepository();
        }

        public AuditService(AuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public void LogCreate<T>(string tableName, int recordId, T newRecord, int? userId = null, string? ipAddress = null)
        {
            try
            {
                var newValues = JsonSerializer.Serialize(newRecord, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                _auditLogRepository.LogAction(tableName, recordId, "INSERT", null, newValues, userId, ipAddress);
            }
            catch (Exception ex)
            {
                // Log error but don't throw to avoid breaking main operations
                Console.WriteLine($"Audit logging failed for CREATE operation: {ex.Message}");
            }
        }

        public void LogUpdate<T>(string tableName, int recordId, T oldRecord, T newRecord, int? userId = null, string? ipAddress = null)
        {
            try
            {
                var oldValues = JsonSerializer.Serialize(oldRecord, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                var newValues = JsonSerializer.Serialize(newRecord, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                _auditLogRepository.LogAction(tableName, recordId, "UPDATE", oldValues, newValues, userId, ipAddress);
            }
            catch (Exception ex)
            {
                // Log error but don't throw to avoid breaking main operations
                Console.WriteLine($"Audit logging failed for UPDATE operation: {ex.Message}");
            }
        }

        public void LogDelete<T>(string tableName, int recordId, T deletedRecord, int? userId = null, string? ipAddress = null)
        {
            try
            {
                var oldValues = JsonSerializer.Serialize(deletedRecord, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                _auditLogRepository.LogAction(tableName, recordId, "DELETE", oldValues, null, userId, ipAddress);
            }
            catch (Exception ex)
            {
                // Log error but don't throw to avoid breaking main operations
                Console.WriteLine($"Audit logging failed for DELETE operation: {ex.Message}");
            }
        }

        public void LogJobStatusChange(int jobId, string oldStatus, string newStatus, int? userId = null, 
            string? reason = null, string? ipAddress = null)
        {
            try
            {
                var statusChange = new
                {
                    JobId = jobId,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    Reason = reason,
                    ChangedAt = DateTime.Now
                };

                var changeDetails = JsonSerializer.Serialize(statusChange, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                _auditLogRepository.LogAction("jobs", jobId, "STATUS_CHANGE", oldStatus, changeDetails, userId, ipAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audit logging failed for job status change: {ex.Message}");
            }
        }

        public void LogUserLogin(int userId, bool successful, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                var loginAttempt = new
                {
                    UserId = userId,
                    Successful = successful,
                    LoginTime = DateTime.Now,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                var loginDetails = JsonSerializer.Serialize(loginAttempt, new JsonSerializerOptions 
                { 
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                
                string actionType = successful ? "LOGIN_SUCCESS" : "LOGIN_FAILED";
                _auditLogRepository.LogAction("users", userId, actionType, null, loginDetails, userId, ipAddress, userAgent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audit logging failed for user login: {ex.Message}");
            }
        }

        public List<AuditLog> GetAuditLogs(int pageNumber = 1, int pageSize = 50, string? tableName = null, 
            string? actionType = null, int? changedBy = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return _auditLogRepository.GetAuditLogs(pageNumber, pageSize, tableName, actionType, changedBy, fromDate, toDate);
        }

        public int GetAuditLogsCount(string? tableName = null, string? actionType = null, 
            int? changedBy = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return _auditLogRepository.GetAuditLogsCount(tableName, actionType, changedBy, fromDate, toDate);
        }

        public List<AuditLog> GetAuditLogsForRecord(string tableName, int recordId)
        {
            return _auditLogRepository.GetAuditLogsForRecord(tableName, recordId);
        }

        public List<string> GetAuditedTables()
        {
            return _auditLogRepository.GetAuditedTables();
        }

        public void CleanupOldAuditLogs(int daysToKeep = 365)
        {
            _auditLogRepository.CleanupOldAuditLogs(daysToKeep);
        }

        // Utility method to get table name from model type
        public static string GetTableName<T>()
        {
            var typeName = typeof(T).Name.ToLower();
            return typeName switch
            {
                "user" => "users",
                "customer" => "customers",
                "job" => "jobs",
                "load" => "loads",
                "quote" => "quotes",
                "vehicle" => "vehicles",
                "staff" => "staff",
                "driver" => "drivers",
                "assistant" => "assistants",
                "container" => "containers",
                "product" => "products",
                "transportunit" => "transport_units",
                _ => typeName + "s" // Default pluralization
            };
        }
    }
}