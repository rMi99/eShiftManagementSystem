using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        void UpdatePayment(Payment payment);
        void DeletePayment(int paymentId);
        Payment GetPaymentById(int paymentId);
        List<Payment> GetAllPayments();
        List<Payment> GetPaymentsByJobId(int jobId);
    }
}
