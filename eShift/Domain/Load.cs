using System;

namespace eShift.Domain
{
    public class Load
    {
        public int LoadID { get; set; }
        public int JobID { get; set; }
        public string Description { get; set; } // e.g., "Box of books", "Antique Vase"
        public decimal Weight { get; set; } // in kg or lbs - consistency is key
        public decimal Volume { get; set; } // in cubic meters or cubic feet
        public bool IsFragile { get; set; }
        public string SpecialHandlingNotes { get; set; } // e.g., "Keep upright", "Protect from moisture"
        public string Category { get; set; } // e.g., Furniture, Electronics, Kitchenware
        public int Quantity { get; set; } // Number of items of this type
        public decimal DeclaredValue { get; set; } // For insurance purposes
        public bool IsDeleted { get; set; } // For soft deletion
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        // public virtual Job Job { get; set; }

        public Load()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Quantity = 1; // Default quantity
        }
    }
}
