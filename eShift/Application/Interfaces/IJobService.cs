using eShift.Domain; // For Job, JobStatus etc.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShift.Application.Interfaces
{
    public interface IJobService
    {
        Task<Job> GetJobByIdAsync(int jobId, int requestingUserId, UserRole requestingUserRole);
        Task<IEnumerable<Job>> GetAllJobsAsync(int requestingUserId, UserRole requestingUserRole); // Admins see all, customers their own
        Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(int customerId, int requestingUserId, UserRole requestingUserRole);
        Task CreateJobAsync(Job job, int creatorUserId);
        Task UpdateJobAsync(Job job, int updaterUserId);
        Task DeleteJobAsync(int jobId, int deleterUserId); // Likely soft delete
        Task UpdateJobStatusAsync(int jobId, JobStatus newStatus, int adminUserId, string remarks = null);

        Task<IEnumerable<Job>> SearchJobsAsync(string searchTerm, JobStatus? status, int? customerIdToFilterBy, int requestingUserId, UserRole requestingUserRole);

        // Load Management within a Job context
        Task AddLoadToJobAsync(int jobId, Load load, int userId);
        Task UpdateLoadAsync(int jobId, Load load, int userId); // jobId for context/security
        Task RemoveLoadFromJobAsync(int jobId, int loadId, int userId);
        Task<IEnumerable<Load>> GetLoadsForJobAsync(int jobId, int requestingUserId, UserRole requestingUserRole);
        Task<Load> GetLoadByIdAsync(int loadId, int requestingUserId, UserRole requestingUserRole);
    }
}
