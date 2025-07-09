using System;

namespace eShiftManagementSystem.Models
{
    public class Assistant
    {
        public int AssistantId { get; set; }
        public int StaffId { get; set; }
        public string? Specialization { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int? CurrentJobId { get; set; }
        public decimal Rating { get; set; } = 0.00m;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Staff? Staff { get; set; }
        public Job? CurrentJob { get; set; }

        // Computed properties
        public string FullName => Staff?.FullName ?? "Unknown Assistant";
        public string DisplayName => $"{FullName}" + (string.IsNullOrEmpty(Specialization) ? "" : $" ({Specialization})");
        public string AvailabilityStatus => IsAvailable ? "Available" : "Assigned";
        public string FormattedRating => Rating > 0 ? $"{Rating:F1}/5.0" : "Not Rated";
        public bool IsCurrentlyAssigned => CurrentJobId.HasValue;
        public string SpecializationDisplay => string.IsNullOrEmpty(Specialization) ? "General" : Specialization;
    }
}