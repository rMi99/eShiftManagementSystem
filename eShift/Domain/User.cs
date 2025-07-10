using System;

namespace eShift.Domain
{
    public enum UserRole
    {
        Admin,
        Customer
    }

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Store hashed passwords only
        public UserRole Role { get; set; }
        public int? CustomerID { get; set; } // Nullable, as Admins might not be Customers
        public bool IsActive { get; set; } // For soft deletes or disabling accounts
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property (optional, depending on ORM or repository design)
        // public virtual Customer Customer { get; set; }
    }
}
