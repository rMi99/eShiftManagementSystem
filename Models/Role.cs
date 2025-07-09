using System;

namespace eShiftManagementSystem.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Computed properties
        public string DisplayName => RoleName.Replace("_", " ").ToTitleCase();
        public bool IsSuperAdmin => RoleName.ToLower() == "super_admin";
        public bool IsAdmin => RoleName.ToLower() == "admin" || IsSuperAdmin;
        public bool IsCustomer => RoleName.ToLower() == "customer";
        public bool IsDriver => RoleName.ToLower() == "driver";
        public bool IsStaff => RoleName.ToLower() == "staff" || IsAdmin;
    }
}