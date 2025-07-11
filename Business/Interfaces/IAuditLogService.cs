using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Interfaces
{
    public interface IAuditLogService
    {
        void AddLog(AuditLog log);
        List<AuditLog> GetAllLogs();
        List<AuditLog> GetLogsByCriteria(DateTime? startDate, DateTime? endDate, int? userId, string actionType);
    }
}