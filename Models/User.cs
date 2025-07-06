using System;
using eShiftManagementSystem.Utils;

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
        public DateTime CreatedAt { get; set; } = DateTimeHelper.Now;
        public DateTime UpdatedAt { get; set; } = DateTimeHelper.Now; // Added this property
        public DateTime? LastLogin { get; set; }

        // Computed properties
        public string DisplayName => !string.IsNullOrEmpty(Username) ? Username : Email;
        public bool IsAdmin => string.Equals(Role, Constants.Roles.Admin, StringComparison.OrdinalIgnoreCase);
        public bool IsCustomer => string.Equals(Role, Constants.Roles.Customer, StringComparison.OrdinalIgnoreCase);
        public bool IsDriver => string.Equals(Role, Constants.Roles.Driver, StringComparison.OrdinalIgnoreCase);
        public bool IsStaff => string.Equals(Role, Constants.Roles.Staff, StringComparison.OrdinalIgnoreCase) || IsAdmin;
    }
}