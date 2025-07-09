using System;

namespace eShiftManagementSystem.Models
{
    public class VehicleType
    {
        public int VehicleTypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public decimal MaxWeightCapacity { get; set; }
        public decimal MaxVolumeCapacity { get; set; }
        public decimal CostPerKm { get; set; } = 0.00m;
        public string? Description { get; set; }

        // Computed properties
        public string DisplayName => TypeName;
        public string CapacityInfo => $"Weight: {MaxWeightCapacity:N0}kg, Volume: {MaxVolumeCapacity:N1}m³";
        public string FormattedCostPerKm => $"PKR {CostPerKm:N2}/km";
        public bool IsSuitableForWeight(decimal weight) => weight <= MaxWeightCapacity;
        public bool IsSuitableForVolume(decimal volume) => volume <= MaxVolumeCapacity;
        public bool IsSuitableFor(decimal weight, decimal volume) => 
            IsSuitableForWeight(weight) && IsSuitableForVolume(volume);
    }
}