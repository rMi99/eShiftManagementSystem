using System;

namespace eShiftManagementSystem.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public int CustomerId { get; set; }
        public string JobNumber { get; set; } = string.Empty;
        public string PickupAddress { get; set; } = string.Empty;
        public string PickupCity { get; set; } = string.Empty;
        public string PickupPostalCode { get; set; } = string.Empty;
        public string DestinationAddress { get; set; } = string.Empty;
        public string DestinationCity { get; set; } = string.Empty;
        public string DestinationPostalCode { get; set; } = string.Empty;
        public DateTime RequestedPickupDate { get; set; } = DateTime.Now.Date;
        public DateTime? RequestedDeliveryDate { get; set; }
        public string Status { get; set; } = "pending";
        public decimal? TotalEstimatedWeight { get; set; }
        public decimal? TotalEstimatedVolume { get; set; }
        public string SpecialInstructions { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // New required properties per problem statement
        public string Priority { get; set; } = "normal";
        public string CustomerName { get; set; } = string.Empty;
        public int? DriverId { get; set; }

        // Navigation property
        public Customer? Customer { get; set; }
    }
}