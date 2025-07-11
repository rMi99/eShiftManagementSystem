using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AuditLogRepository _auditLogRepository;

        public AuditLogService()
        {
            _auditLogRepository = new AuditLogRepository();
        }

        public void AddLog(AuditLog log)
        {
            _auditLogRepository.AddLog(log);
        }

        public List<AuditLog> GetAllLogs()
        {
            return _auditLogRepository.GetAllLogs();
        }

        public List<AuditLog> GetLogsByCriteria(DateTime? startDate, DateTime? endDate, int? userId, string actionType)
        {
            return _auditLogRepository.GetLogsByCriteria(startDate, endDate, userId, actionType);
        }
    }
}