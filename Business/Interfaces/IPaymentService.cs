using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Interfaces
{
    public interface IPaymentService
    {
        void AddPayment(Payment payment);
        void UpdatePayment(Payment payment, int userId);
        void DeletePayment(int paymentId, int userId);
        List<Payment> GetPaymentsByJobId(int jobId);
        decimal GetTotalPaymentsForJob(int jobId);
    }
}
