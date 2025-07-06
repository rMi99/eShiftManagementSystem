using System;

namespace eShiftManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Added this property
        public DateTime? LastLogin { get; set; }

        // Computed properties
        public string DisplayName => !string.IsNullOrEmpty(Username) ? Username : Email;
        public bool IsAdmin => string.Equals(Role, "admin", StringComparison.OrdinalIgnoreCase);
        public bool IsCustomer => string.Equals(Role, "customer", StringComparison.OrdinalIgnoreCase);
        public bool IsDriver => string.Equals(Role, "driver", StringComparison.OrdinalIgnoreCase);
        public bool IsStaff => string.Equals(Role, "staff", StringComparison.OrdinalIgnoreCase) || IsAdmin;
    }
}