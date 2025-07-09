using System;

namespace eShiftManagementSystem.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public int? AssignedBy { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Role? Role { get; set; }
        public User? AssignedByUser { get; set; }

        // Computed properties
        public string DisplayText => $"{User?.DisplayName} - {Role?.DisplayName}";
        public string AssignedByName => AssignedByUser?.DisplayName ?? "System";
        public string FormattedAssignedDate => AssignedAt.ToString("dd/MM/yyyy");
    }
}