using System;

namespace eShiftManagementSystem.Models
{
    public class JobStatusHistory
    {
        public int HistoryId { get; set; }
        public int JobId { get; set; }
        public string? OldStatus { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public int ChangedBy { get; set; }
        public string? ChangeReason { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        public string? AdditionalNotes { get; set; }

        // Navigation properties
        public Job? Job { get; set; }
        public User? ChangedByUser { get; set; }

        // Computed properties
        public string DisplayMessage => $"Status changed from '{OldStatus ?? "N/A"}' to '{NewStatus}'";
        public string FormattedChangeDate => ChangedAt.ToString("dd/MM/yyyy HH:mm");
    }
}