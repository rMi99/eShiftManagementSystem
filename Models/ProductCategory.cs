using System;

namespace eShiftManagementSystem.Models
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsFragile { get; set; } = false;
        public decimal DefaultHandlingCost { get; set; } = 0.00m;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Computed properties
        public string DisplayName => CategoryName;
        public string FragileStatus => IsFragile ? "Fragile" : "Standard";
        public string FormattedHandlingCost => $"PKR {DefaultHandlingCost:N2}";
    }
}