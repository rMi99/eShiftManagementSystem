using System;

namespace eShift.Domain
{
    public enum ContainerStatus
    {
        Available,
        InUse,
        Damaged,
        Unavailable
    }

    public class Container
    {
        public int ContainerID { get; set; }
        public string ContainerIdentifier { get; set; } // e.g., C001, BOX-LARGE-002
        public string Type { get; set; } // e.g., "Standard Box", "Wardrobe Box", "Packing Crate"
        public decimal CapacityVolume { get; set; } // Optional, if containers have defined volumes
        public decimal CapacityWeight { get; set; } // Optional, if containers have weight limits
        public string Dimensions { get; set; } // e.g., "50x50x50 cm"
        public ContainerStatus Status { get; set; }
        public bool IsReusable { get; set; }
        public bool IsActive { get; set; } // For soft deletion or retiring a container type
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Container()
        {
            Status = ContainerStatus.Available;
            IsReusable = true;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
