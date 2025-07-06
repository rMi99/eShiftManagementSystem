using System;
using eShiftManagementSystem.Utils;

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
        public DateTime RequestedPickupDate { get; set; } = DateTimeHelper.Today;
        public DateTime? RequestedDeliveryDate { get; set; }
        public string Status { get; set; } = Constants.JobStatus.Pending;
        public decimal? TotalEstimatedWeight { get; set; }
        public decimal? TotalEstimatedVolume { get; set; }
        public string SpecialInstructions { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTimeHelper.Now;
        public DateTime UpdatedAt { get; set; } = DateTimeHelper.Now;

        // New required properties per problem statement
        public string Priority { get; set; } = Constants.JobPriority.Normal;
        public string CustomerName { get; set; } = string.Empty;
        public int? DriverId { get; set; }

        // Navigation property
        public Customer? Customer { get; set; }
    }
}