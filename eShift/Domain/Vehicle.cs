using System;

namespace eShift.Domain
{
    public enum VehicleStatus
    {
        Available,
        InUse,
        Maintenance,
        Unavailable
    }

    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; } // e.g., "Ford Transit", "Mercedes Sprinter"
        public string Type { get; set; } // e.g., "Van", "Truck (10ft)", "Truck (26ft)"
        public decimal CapacityWeight { get; set; } // Max weight in kg/lbs
        public decimal CapacityVolume { get; set; } // Max volume in m^3/ft^3
        public VehicleStatus Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Mileage { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public bool IsActive { get; set; } // For soft deletion or retiring a vehicle
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Vehicle()
        {
            Status = VehicleStatus.Available;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
