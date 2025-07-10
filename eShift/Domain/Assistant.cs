using System;

namespace eShift.Domain
{
    // EmployeeStatus enum is already defined in Driver.cs, assuming it's in the same namespace
    // or will be appropriately referenced.

    public class Assistant
    {
        public int AssistantID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public EmployeeStatus Status { get; set; } // Using the same enum as Driver
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } // For soft deletion
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public Assistant()
        {
            Status = EmployeeStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
