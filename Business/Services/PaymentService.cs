using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace eShiftManagementSystem.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly AuditLogService _auditLogService = new AuditLogService();

        public PaymentService()
        {
            _paymentRepository = new PaymentRepository();
        }

        public void AddPayment(Payment payment)
        {
            _paymentRepository.AddPayment(payment);
        }

        public void UpdatePayment(Payment payment, int userId)
        {
            _paymentRepository.UpdatePayment(payment);
            _auditLogService.AddLog(new AuditLog
            {
                UserId = userId,
                Action = "UPDATE",
                TableAffected = "payments",
                RecordId = payment.PaymentId,
                NewValues = $"Amount: {payment.Amount}, Method: {payment.PaymentMethod}, Status: {payment.PaymentStatus}, TransactionID: {payment.TransactionId}, Date: {payment.PaymentDate}"
            });
        }

        public void DeletePayment(int paymentId, int userId)
        {
            _paymentRepository.DeletePayment(paymentId);
            _auditLogService.AddLog(new AuditLog
            {
                UserId = userId,
                Action = "DELETE",
                TableAffected = "payments",
                RecordId = paymentId
            });
        }

        public List<Payment> GetPaymentsByJobId(int jobId)
        {
            return _paymentRepository.GetPaymentsByJobId(jobId);
        }

        public decimal GetTotalPaymentsForJob(int jobId)
        {
            var payments = _paymentRepository.GetPaymentsByJobId(jobId);
            return payments.Where(p => p.PaymentStatus.ToLower() == "paid" || p.PaymentStatus.ToLower() == "advance").Sum(p => p.Amount);
        }
    }
}
