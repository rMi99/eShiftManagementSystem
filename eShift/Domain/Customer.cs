using System;
using System.Collections.Generic;

namespace eShift.Domain
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; } // For soft deletion
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties (optional, depending on ORM or repository design)
        // public virtual User User { get; set; } // If a customer must have a user account
        // public virtual ICollection<Job> Jobs { get; set; }

        public Customer()
        {
            // Jobs = new HashSet<Job>();
        }
    }
}
