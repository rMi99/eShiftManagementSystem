using System;
using System.Collections.Generic;

namespace eShift.Domain
{
    public enum JobStatus
    {
        PendingApproval, // Customer created, Admin to review
        Accepted,        // Admin approved
        Declined,        // Admin declined
        InProgress,      // Transport unit assigned, job underway
        Completed,       // All loads delivered
        CancelledByCustomer,
        CancelledByAdmin
    }

    public class Job
    {
        public int JobID { get; set; }
        public int CustomerID { get; set; }
        public string PickupAddressLine1 { get; set; }
        public string PickupAddressLine2 { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string PickupPostalCode { get; set; }
        public string PickupCountry { get; set; }
        public string DeliveryAddressLine1 { get; set; }
        public string DeliveryAddressLine2 { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryPostalCode { get; set; }
        public string DeliveryCountry { get; set; }
        public DateTime JobRequestDate { get; set; } // When the customer requested the job
        public DateTime? PreferredPickupDate { get; set; }
        public DateTime? PreferredDeliveryDate { get; set; }
        public JobStatus Status { get; set; }
        public string Remarks { get; set; } // Any special instructions or notes from customer or admin
        public decimal TotalCost { get; set; } // Could be calculated or set by Admin
        public bool IsDeleted { get; set; } // For soft deletion
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        // public virtual Customer Customer { get; set; }
        // public virtual ICollection<Load> Loads { get; set; }
        // public virtual TransportUnit TransportUnit { get; set; }

        public Job()
        {
            // Loads = new HashSet<Load>();
            Status = JobStatus.PendingApproval; // Default status
            JobRequestDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
