using System;

namespace eShiftManagementSystem.Models
{
    public class Driver
    {
        public int DriverId { get; set; }
        public int StaffId { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public int ExperienceYears { get; set; } = 0;
        public string? VehicleTypePreference { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int? CurrentVehicleId { get; set; }
        public decimal Rating { get; set; } = 0.00m;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Staff? Staff { get; set; }
        public Vehicle? CurrentVehicle { get; set; }

        // Computed properties
        public string FullName => Staff?.FullName ?? "Unknown Driver";
        public string DisplayName => $"{FullName} ({LicenseNumber})";
        public bool IsLicenseValid => LicenseExpiryDate > DateTime.Now;
        public bool IsExperienced => ExperienceYears >= 3;
        public string AvailabilityStatus => IsAvailable ? "Available" : "Busy";
        public string ExperienceLevel => ExperienceYears switch
        {
            < 1 => "Beginner",
            < 3 => "Junior",
            < 5 => "Experienced",
            _ => "Senior"
        };
        public string FormattedRating => Rating > 0 ? $"{Rating:F1}/5.0" : "Not Rated";
        public int DaysUntilLicenseExpiry => (int)(LicenseExpiryDate - DateTime.Now).TotalDays;
        public bool NeedsLicenseRenewal => DaysUntilLicenseExpiry <= 30;
    }
}