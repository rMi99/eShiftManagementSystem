using System;
using eShiftManagementSystem.Utils;

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
        public string Status { get; set; } = Constants.QuoteStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTimeHelper.Now;
        public DateTime UpdatedAt { get; set; } = DateTimeHelper.Now;

        // Navigation property
        public Customer? Customer { get; set; }

        // Computed properties
        public bool IsExpired => DateTime.Now > ValidUntil;
        public bool IsAccepted => string.Equals(Status, Constants.QuoteStatus.Accepted, StringComparison.OrdinalIgnoreCase);
        public bool IsPending => string.Equals(Status, Constants.QuoteStatus.Pending, StringComparison.OrdinalIgnoreCase);
        public bool IsDeclined => string.Equals(Status, Constants.QuoteStatus.Declined, StringComparison.OrdinalIgnoreCase);
    }
}