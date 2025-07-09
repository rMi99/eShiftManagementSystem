using System;

namespace eShiftManagementSystem.Models
{
    public class LoadProduct
    {
        public int LoadProductId { get; set; }
        public int LoadId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? UnitWeight { get; set; }
        public decimal? UnitVolume { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalVolume { get; set; }
        public string? ConditionNotes { get; set; }

        // Navigation properties
        public Load? Load { get; set; }
        public Product? Product { get; set; }

        // Computed properties
        public decimal CalculatedTotalWeight => (UnitWeight ?? 0) * Quantity;
        public decimal CalculatedTotalVolume => (UnitVolume ?? 0) * Quantity;
        public string DisplayName => $"{Product?.ProductName} (Qty: {Quantity})";
        public bool HasDamageNotes => !string.IsNullOrWhiteSpace(ConditionNotes);
    }
}