using System;

namespace eShift.Domain
{
    public enum TransportUnitStatus
    {
        Planned,    // Resources assigned, not yet started
        EnRouteToPickup,
        AtPickupLocation,
        Loading,
        EnRouteToDelivery,
        AtDeliveryLocation,
        Unloading,
        Completed,
        Delayed,
        Cancelled
    }

    public class TransportUnit
    {
        public int TransportUnitID { get; set; }
        public int JobID { get; set; } // Foreign key to Job
        public int? VehicleID { get; set; } // Nullable if a job can be planned without immediate vehicle assignment
        public int? DriverID { get; set; }  // Nullable
        public int? AssistantID { get; set; } // Nullable, as not all jobs may need an assistant
        public int? ContainerID { get; set; } // Nullable, if containers are optional or tracked differently

        public DateTime? ActualDepartureTime { get; set; } // When the unit actually left
        public DateTime? ActualArrivalTimeAtPickup { get; set; }
        public DateTime? ActualDepartureTimeFromPickup { get; set; }
        public DateTime? ActualArrivalTimeAtDelivery { get; set; }
        public DateTime? ActualCompletionTime { get; set; } // When the job was fully completed by this unit

        public TransportUnitStatus Status { get; set; }
        public string Notes { get; set; } // Any operational notes
        public bool IsActive { get; set; } // For soft deletion
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        // public virtual Job Job { get; set; }
        // public virtual Vehicle Vehicle { get; set; }
        // public virtual Driver Driver { get; set; }
        // public virtual Assistant Assistant { get; set; }
        // public virtual Container Container { get; set; }

        public TransportUnit()
        {
            Status = TransportUnitStatus.Planned;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
