using System;

namespace eShift.Domain
{
    public enum AuditActionType
    {
        Create,
        Update,
        Delete,
        Login, // Could also be an audit action
        Logout,
        View,  // If viewing sensitive data needs to be logged
        ReportGeneration
    }

    public class AuditLog
    {
        public long LogID { get; set; } // Using long for potentially many entries
        public int? UserID { get; set; } // Nullable if system actions are logged
        public string Username { get; set; } // Denormalized for easier viewing, but UserID is key
        public AuditActionType ActionType { get; set; } // e.g., "Create", "Update", "Delete"
        public string EntityName { get; set; } // e.g., "Job", "Load", "Customer"
        public int? EntityID { get; set; } // The ID of the affected entity
        public DateTime Timestamp { get; set; }
        public string Changes { get; set; } // Detailed changes, potentially JSON or XML formatted string
                                           // e.g., "Status changed from 'Pending' to 'Approved'"
                                           // or a serialized representation of old and new values.
        public string AffectedRole { get; set; } // Role of the user performing the action (Admin/Customer)
        public string IpAddress { get; set; } // Optional: IP address of the user

        public AuditLog()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
