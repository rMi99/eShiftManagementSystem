using System;

namespace eShiftManagementSystem.Models
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; }
        public string TableAffected { get; set; }
        public int? RecordId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime ActionTimestamp { get; set; }
        public string IpAddress { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}