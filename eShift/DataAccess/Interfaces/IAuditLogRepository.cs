using eShift.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace eShift.DataAccess.Interfaces
{
    public interface IAuditLogRepository // AuditLog typically only needs Add and Get (no Update/Delete from app)
    {
        Task AddAsync(AuditLog logEntry);
        Task<IEnumerable<AuditLog>> GetAllAsync(); // Or with filters like date range, user, etc.
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId);
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityName, int? entityId);
    }
}
