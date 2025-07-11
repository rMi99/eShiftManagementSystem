using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IAuditLogRepository
    {
        void AddLog(AuditLog log);
        List<AuditLog> GetAllLogs();
        List<AuditLog> GetLogsByCriteria(DateTime? startDate, DateTime? endDate, int? userId, string actionType);
    }
}