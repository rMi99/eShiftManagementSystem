using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public void AddPayment(Payment payment)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO payments (job_id, amount, payment_date, payment_method, payment_status, transaction_id, created_at) 
                              VALUES (@JobId, @Amount, @PaymentDate, @PaymentMethod, @PaymentStatus, @TransactionId, NOW())";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", payment.JobId);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    command.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                    command.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);
                    command.Parameters.AddWithValue("@TransactionId", payment.TransactionId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePayment(Payment payment)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                var query = @"UPDATE payments SET 
                                  job_id = @JobId,
                                  amount = @Amount, 
                                  payment_method = @PaymentMethod, 
                                  payment_status = @PaymentStatus, 
                                  transaction_id = @TransactionId, 
                                  payment_date = @PaymentDate, 
                                  updated_at = NOW() 
                              WHERE payment_id = @PaymentId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", payment.JobId);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);
                    command.Parameters.AddWithValue("@PaymentStatus", payment.PaymentStatus);
                    command.Parameters.AddWithValue("@TransactionId", payment.TransactionId);
                    command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    command.Parameters.AddWithValue("@PaymentId", payment.PaymentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeletePayment(int paymentId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                var query = "DELETE FROM payments WHERE payment_id = @PaymentId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PaymentId", paymentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Payment GetPaymentById(int paymentId)
        {
            // Implementation to get a single payment by ID
            // ...
            return null; // Simplified for now
        }

        public List<Payment> GetAllPayments()
        {
            // Implementation to get all payments
            // ...
            return new List<Payment>(); // Simplified for now
        }

        public List<Payment> GetPaymentsByJobId(int jobId)
        {
            var payments = new List<Payment>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM payments WHERE job_id = @JobId ORDER BY payment_date DESC";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", jobId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment
                            {
                                PaymentId = Convert.ToInt32(reader["payment_id"]),
                                JobId = Convert.ToInt32(reader["job_id"]),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                PaymentDate = Convert.ToDateTime(reader["payment_date"]),
                                PaymentMethod = reader["payment_method"].ToString(),
                                PaymentStatus = reader["payment_status"].ToString(),
                                TransactionId = reader["transaction_id"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            });
                        }
                    }
                }
            }
            return payments;
        }
    }
}
