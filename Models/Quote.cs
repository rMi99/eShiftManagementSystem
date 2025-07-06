using System;

namespace eShiftManagementSystem.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }
        public int CustomerId { get; set; }
        public string QuoteNumber { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ValidUntil { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public Customer? Customer { get; set; }

        // Computed properties
        public bool IsExpired => DateTime.Now > ValidUntil;
        public bool IsAccepted => string.Equals(Status, "accepted", StringComparison.OrdinalIgnoreCase);
        public bool IsPending => string.Equals(Status, "pending", StringComparison.OrdinalIgnoreCase);
        public bool IsDeclined => string.Equals(Status, "declined", StringComparison.OrdinalIgnoreCase);
    }
}