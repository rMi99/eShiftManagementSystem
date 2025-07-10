using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int JobId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public Job Job { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}