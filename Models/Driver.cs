using System;

namespace eShiftManagementSystem.Models
{
    /// <summary>
    /// Represents a driver in the e-Shift Management System
    /// </summary>
    public class Driver
    {
        public int DriverId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now.Date;
        public bool IsActive { get; set; } = true;
        public string Status { get; set; } = "available"; // available, on_job, unavailable
        public string VehicleAssignment { get; set; } = string.Empty;

        // Navigation property
        public User? User { get; set; }

        // Computed property
        public string FullName 
        { 
            get { return $"{FirstName} {LastName}".Trim(); } 
        }
    }
}