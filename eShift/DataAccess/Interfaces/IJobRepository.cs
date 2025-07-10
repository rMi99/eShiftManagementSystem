using eShift.Domain; // Required for Job entity
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShift.DataAccess.Interfaces
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(int customerId);
        Task<IEnumerable<Job>> GetJobsByStatusAsync(JobStatus status);
        Task<IEnumerable<Job>> SearchJobsAsync(string searchTerm, JobStatus? status, int? customerId);
        // Add other job-specific methods, e.g., getting jobs within a date range
    }
}
