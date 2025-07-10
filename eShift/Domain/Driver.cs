using System;

namespace eShift.Domain
{
    public enum EmployeeStatus // Can be used for Drivers and Assistants
    {
        Active,
        OnLeave,
        Inactive
    }

    public class Driver
    {
        public int DriverID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } // For soft deletion
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public Driver()
        {
            Status = EmployeeStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
