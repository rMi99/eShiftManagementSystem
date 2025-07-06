using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eShiftManagementSystem.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportRepository _reportRepository;
        private readonly JobRepository _jobRepository;
        private readonly CustomerRepository _customerRepository;

        public ReportService()
        {
            _reportRepository = new ReportRepository();
            _jobRepository = new JobRepository();
            _customerRepository = new CustomerRepository();
        }

        public List<Job> GetJobsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _reportRepository.GetJobsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving jobs by date range: {ex.Message}");
            }
        }

        public List<Customer> GetCustomersByRegistrationDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _reportRepository.GetCustomersByRegistrationDate(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customers by registration date: {ex.Message}");
            }
        }

        public Dictionary<string, int> GetJobStatusStatistics()
        {
            try
            {
                return _reportRepository.GetJobStatusStatistics();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving job status statistics: {ex.Message}");
            }
        }

        public Dictionary<string, decimal> GetRevenueByMonth(int year)
        {
            try
            {
                return _reportRepository.GetRevenueByMonth(year);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving revenue by month: {ex.Message}");
            }
        }

        public List<object> GetTopCustomers(int count)
        {
            try
            {
                return _reportRepository.GetTopCustomers(count);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving top customers: {ex.Message}");
            }
        }

        public Report GenerateCustomerReport(int customerId)
        {
            try
            {
                var customer = _customerRepository.GetCustomerById(customerId);
                if (customer is null)
                {
                    throw new ArgumentException("Customer not found.");
                }

                var jobs = _jobRepository.GetJobsByCustomerId(customerId);
                
                var report = new Report
                {
                    Title = $"Customer Report - {customer.FullName}",
                    Type = "Customer",
                    GeneratedAt = DateTime.Now,
                    Content = GenerateCustomerReportContent(customer, jobs)
                };

                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating customer report: {ex.Message}");
            }
        }

        public Report GenerateJobReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var jobs = GetJobsByDateRange(startDate, endDate);
                
                var report = new Report
                {
                    Title = $"Job Report ({startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy})",
                    Type = "Job",
                    GeneratedAt = DateTime.Now,
                    StartDate = startDate,
                    EndDate = endDate,
                    Content = GenerateJobReportContent(jobs)
                };

                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating job report: {ex.Message}");
            }
        }

        public Report GenerateRevenueReport(int year)
        {
            try
            {
                var revenue = GetRevenueByMonth(year);
                
                var report = new Report
                {
                    Title = $"Revenue Report - {year}",
                    Type = "Revenue",
                    GeneratedAt = DateTime.Now,
                    Content = GenerateRevenueReportContent(revenue, year)
                };

                return report;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating revenue report: {ex.Message}");
            }
        }

        private string GenerateCustomerReportContent(Customer customer, List<Job> jobs)
        {
            var content = $"Customer: {customer.FullName}\n";
            content += $"Email: {(customer.User != null ? customer.User.Email : "N/A")}\n";
            content += $"Phone: {customer.Phone}\n";
            content += $"Address: {customer.Address}, {customer.City}, {customer.PostalCode}\n";
            content += $"Registration Date: {customer.RegistrationDate:dd/MM/yyyy}\n\n";
            content += $"Total Jobs: {jobs.Count}\n";
            content += $"Completed Jobs: {jobs.Count(j => j.Status == "completed")}\n";
            content += $"Pending Jobs: {jobs.Count(j => j.Status == "pending")}\n";
            
            return content;
        }

        private string GenerateJobReportContent(List<Job> jobs)
        {
            var content = $"Total Jobs: {jobs.Count}\n";
            content += $"Completed: {jobs.Count(j => j.Status == "completed")}\n";
            content += $"Pending: {jobs.Count(j => j.Status == "pending")}\n";
            content += $"In Progress: {jobs.Count(j => j.Status == "in_progress")}\n";
            content += $"Cancelled: {jobs.Count(j => j.Status == "cancelled")}\n\n";
            
            var monthlyStats = jobs.GroupBy(j => j.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(x => x.Month);
            
            content += "Monthly Breakdown:\n";
            foreach (var stat in monthlyStats)
            {
                content += $"Month {stat.Month}: {stat.Count} jobs\n";
            }
            
            return content;
        }

        private string GenerateRevenueReportContent(Dictionary<string, decimal> revenue, int year)
        {
            var content = $"Revenue Report for {year}\n\n";
            var totalRevenue = revenue.Values.Sum();
            content += $"Total Annual Revenue: £{totalRevenue:N2}\n\n";
            
            content += "Monthly Breakdown:\n";
            foreach (var item in revenue)
            {
                content += $"{item.Key}: £{item.Value:N2}\n";
            }
            
            if (revenue.Count > 0)
            {
                var avgMonthly = totalRevenue / revenue.Count;
                content += $"\nAverage Monthly Revenue: £{avgMonthly:N2}\n";
            }
            
            return content;
        }
    }
}