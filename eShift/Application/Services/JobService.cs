using eShift.Application.Interfaces;
using eShift.DataAccess.Interfaces;
using eShift.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication; // For unauthorized access
using System.Threading.Tasks;

namespace eShift.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ILoadRepository _loadRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUserRepository _userRepository; // To get customer ID from user ID

        public JobService(IJobRepository jobRepository, ILoadRepository loadRepository, IAuditLogRepository auditLogRepository, IUserRepository userRepository)
        {
            _jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository));
            _loadRepository = loadRepository ?? throw new ArgumentNullException(nameof(loadRepository));
            _auditLogRepository = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        private async Task<User> GetUserAndValidate(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new AuthenticationException("User not found.");
            return user;
        }

        private async Task LogAction(int userId, AuditActionType actionType, string entityName, int? entityId, string changes, UserRole role)
        {
            var user = await _userRepository.GetByIdAsync(userId); // Assuming GetByIdAsync is available
            await _auditLogRepository.AddAsync(new AuditLog
            {
                UserID = userId,
                Username = user?.Username ?? "System",
                ActionType = actionType,
                EntityName = entityName,
                EntityID = entityId,
                Changes = changes,
                Timestamp = DateTime.UtcNow,
                AffectedRole = role.ToString()
            });
        }

        public async Task<Job> GetJobByIdAsync(int jobId, int requestingUserId, UserRole requestingUserRole)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null || job.IsDeleted) return null;

            if (requestingUserRole == UserRole.Customer)
            {
                var requestingUser = await GetUserAndValidate(requestingUserId);
                if (job.CustomerID != requestingUser.CustomerID)
                {
                    await LogAction(requestingUserId, AuditActionType.View, "Job", jobId, "Unauthorized attempt to view job.", requestingUserRole);
                    throw new UnauthorizedAccessException("Customers can only view their own jobs.");
                }
            }
            // Admins can view any job.
            return job;
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync(int requestingUserId, UserRole requestingUserRole)
        {
            if (requestingUserRole == UserRole.Admin)
            {
                return await _jobRepository.GetAllAsync(); // Assumes GetAllAsync filters IsDeleted=false
            }
            else // Customer
            {
                var requestingUser = await GetUserAndValidate(requestingUserId);
                if (!requestingUser.CustomerID.HasValue)
                    throw new InvalidOperationException("Customer user does not have an associated CustomerID.");
                return await _jobRepository.GetJobsByCustomerIdAsync(requestingUser.CustomerID.Value);
            }
        }

        public async Task<IEnumerable<Job>> GetJobsByCustomerIdAsync(int customerId, int requestingUserId, UserRole requestingUserRole)
        {
            if (requestingUserRole == UserRole.Admin)
            {
                 return await _jobRepository.GetJobsByCustomerIdAsync(customerId);
            }
            else // Customer
            {
                var requestingUser = await GetUserAndValidate(requestingUserId);
                if (!requestingUser.CustomerID.HasValue || requestingUser.CustomerID.Value != customerId)
                {
                     throw new UnauthorizedAccessException("Customers can only view their own jobs.");
                }
                return await _jobRepository.GetJobsByCustomerIdAsync(customerId);
            }
        }


        public async Task CreateJobAsync(Job job, int creatorUserId)
        {
            var creator = await GetUserAndValidate(creatorUserId);

            if (creator.Role == UserRole.Customer)
            {
                if (!creator.CustomerID.HasValue)
                    throw new InvalidOperationException("Customer user must have an associated CustomerID to create a job.");
                job.CustomerID = creator.CustomerID.Value; // Ensure job is linked to the customer
            }
            // Admins can create jobs for any customer, CustomerID should be set in 'job' object by admin.

            job.Status = JobStatus.PendingApproval; // Initial status
            job.CreatedAt = DateTime.UtcNow;
            job.UpdatedAt = DateTime.UtcNow;
            job.IsDeleted = false;

            await _jobRepository.AddAsync(job);
            await LogAction(creatorUserId, AuditActionType.Create, "Job", job.JobID, $"Job created. Pickup: {job.PickupCity}, Delivery: {job.DeliveryCity}", creator.Role);
        }

        public async Task UpdateJobAsync(Job jobUpdates, int updaterUserId)
        {
            var updater = await GetUserAndValidate(updaterUserId);
            var existingJob = await _jobRepository.GetByIdAsync(jobUpdates.JobID);

            if (existingJob == null || existingJob.IsDeleted)
                throw new KeyNotFoundException("Job not found or has been deleted.");

            if (updater.Role == UserRole.Customer)
            {
                if (existingJob.CustomerID != updater.CustomerID)
                    throw new UnauthorizedAccessException("Customers can only update their own jobs.");
                // Customers might only be allowed to update certain fields or before a certain status
                if (existingJob.Status != JobStatus.PendingApproval && existingJob.Status != JobStatus.Accepted) // Example restriction
                     throw new InvalidOperationException($"Job cannot be updated when status is {existingJob.Status}.");
            }

            // Apply updates - be specific about what can be updated
            existingJob.PickupAddressLine1 = jobUpdates.PickupAddressLine1;
            // ... copy other updatable fields ...
            existingJob.DeliveryCity = jobUpdates.DeliveryCity; // Example
            existingJob.Remarks = jobUpdates.Remarks;
            existingJob.PreferredPickupDate = jobUpdates.PreferredPickupDate;
            existingJob.PreferredDeliveryDate = jobUpdates.PreferredDeliveryDate;
            existingJob.UpdatedAt = DateTime.UtcNow;
            // Status updates should go via UpdateJobStatusAsync for admins

            await _jobRepository.UpdateAsync(existingJob);
            await LogAction(updaterUserId, AuditActionType.Update, "Job", existingJob.JobID, "Job details updated.", updater.Role);
        }

        public async Task UpdateJobStatusAsync(int jobId, JobStatus newStatus, int adminUserId, string remarks = null)
        {
            var adminUser = await GetUserAndValidate(adminUserId);
            if (adminUser.Role != UserRole.Admin)
                throw new UnauthorizedAccessException("Only admins can update job status.");

            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null || job.IsDeleted) throw new KeyNotFoundException("Job not found.");

            var oldStatus = job.Status;
            job.Status = newStatus;
            if (!string.IsNullOrEmpty(remarks))
            {
                job.Remarks = string.IsNullOrEmpty(job.Remarks) ? remarks : $"{job.Remarks}\nAdmin ({adminUser.Username}): {remarks}";
            }
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.UpdateAsync(job);
            await LogAction(adminUserId, AuditActionType.Update, "Job", jobId, $"Job status changed from {oldStatus} to {newStatus}. Remarks: {remarks}", UserRole.Admin);
        }


        public async Task DeleteJobAsync(int jobId, int deleterUserId) // Soft delete
        {
            var deleter = await GetUserAndValidate(deleterUserId);
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null) throw new KeyNotFoundException("Job not found.");

            if (deleter.Role == UserRole.Customer)
            {
                if (job.CustomerID != deleter.CustomerID)
                    throw new UnauthorizedAccessException("Customers can only delete their own jobs.");
                // Customers might only be allowed to delete jobs in certain statuses (e.g., PendingApproval)
                if (job.Status != JobStatus.PendingApproval)
                    throw new InvalidOperationException("Job can only be cancelled by customer if it's pending approval.");
                job.Status = JobStatus.CancelledByCustomer; // Or just soft delete
            }

            job.IsDeleted = true; // Soft delete
            job.UpdatedAt = DateTime.UtcNow;
            await _jobRepository.UpdateAsync(job); // Assuming UpdateAsync handles IsDeleted flag
            await LogAction(deleterUserId, AuditActionType.Delete, "Job", jobId, "Job soft deleted.", deleter.Role);
        }


        public async Task<IEnumerable<Job>> SearchJobsAsync(string searchTerm, JobStatus? status, int? customerIdToFilterBy, int requestingUserId, UserRole requestingUserRole)
        {
            var requestingUser = await GetUserAndValidate(requestingUserId);

            if (requestingUserRole == UserRole.Customer)
            {
                if (!requestingUser.CustomerID.HasValue)
                    throw new InvalidOperationException("Customer user must have an associated CustomerID.");
                // Force filter by their own CustomerID
                return await _jobRepository.SearchJobsAsync(searchTerm, status, requestingUser.CustomerID.Value);
            }
            else // Admin
            {
                return await _jobRepository.SearchJobsAsync(searchTerm, status, customerIdToFilterBy);
            }
        }

        // --- Load Management ---

        public async Task AddLoadToJobAsync(int jobId, Load load, int userId)
        {
            var user = await GetUserAndValidate(userId);
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null || job.IsDeleted) throw new KeyNotFoundException("Job not found.");

            if (user.Role == UserRole.Customer && job.CustomerID != user.CustomerID)
                throw new UnauthorizedAccessException("Customers can only add loads to their own jobs.");

            // Add more business rules: e.g., cannot add loads if job is 'Completed'
            if (job.Status == JobStatus.Completed || job.Status == JobStatus.CancelledByAdmin || job.Status == JobStatus.CancelledByCustomer)
                throw new InvalidOperationException($"Cannot add loads to a job with status {job.Status}.");

            load.JobID = jobId;
            load.CreatedAt = DateTime.UtcNow;
            load.UpdatedAt = DateTime.UtcNow;
            load.IsDeleted = false;
            await _loadRepository.AddAsync(load);
            await LogAction(userId, AuditActionType.Create, "Load", load.LoadID, $"Load '{load.Description}' added to Job ID {jobId}.", user.Role);
        }

        public async Task UpdateLoadAsync(int jobId, Load loadUpdates, int userId)
        {
            var user = await GetUserAndValidate(userId);
            var job = await _jobRepository.GetByIdAsync(jobId); // Check job context
            if (job == null || job.IsDeleted) throw new KeyNotFoundException("Job not found for the load.");

            if (user.Role == UserRole.Customer && job.CustomerID != user.CustomerID)
                throw new UnauthorizedAccessException("Customers can only update loads on their own jobs.");

            var existingLoad = await _loadRepository.GetByIdAsync(loadUpdates.LoadID);
            if (existingLoad == null || existingLoad.IsDeleted || existingLoad.JobID != jobId)
                throw new KeyNotFoundException("Load not found or does not belong to this job.");

            // Similar status checks as AddLoadToJobAsync
             if (job.Status == JobStatus.Completed || job.Status == JobStatus.CancelledByAdmin || job.Status == JobStatus.CancelledByCustomer)
                throw new InvalidOperationException($"Cannot update loads for a job with status {job.Status}.");

            // Apply updates
            existingLoad.Description = loadUpdates.Description;
            existingLoad.Weight = loadUpdates.Weight;
            existingLoad.Volume = loadUpdates.Volume;
            existingLoad.IsFragile = loadUpdates.IsFragile;
            existingLoad.SpecialHandlingNotes = loadUpdates.SpecialHandlingNotes;
            existingLoad.Category = loadUpdates.Category;
            existingLoad.Quantity = loadUpdates.Quantity;
            existingLoad.DeclaredValue = loadUpdates.DeclaredValue;
            existingLoad.UpdatedAt = DateTime.UtcNow;

            await _loadRepository.UpdateAsync(existingLoad);
            await LogAction(userId, AuditActionType.Update, "Load", existingLoad.LoadID, $"Load '{existingLoad.Description}' (Job ID {jobId}) updated.", user.Role);
        }

        public async Task RemoveLoadFromJobAsync(int jobId, int loadId, int userId) // Soft delete
        {
            var user = await GetUserAndValidate(userId);
            var job = await _jobRepository.GetByIdAsync(jobId);
             if (job == null || job.IsDeleted) throw new KeyNotFoundException("Job not found for the load.");

            if (user.Role == UserRole.Customer && job.CustomerID != user.CustomerID)
                throw new UnauthorizedAccessException("Customers can only remove loads from their own jobs.");

            var load = await _loadRepository.GetByIdAsync(loadId);
            if (load == null || load.JobID != jobId)
                throw new KeyNotFoundException("Load not found or does not belong to this job.");

            // Similar status checks as AddLoadToJobAsync
            if (job.Status == JobStatus.Completed || job.Status == JobStatus.CancelledByAdmin || job.Status == JobStatus.CancelledByCustomer)
                throw new InvalidOperationException($"Cannot remove loads from a job with status {job.Status}.");

            load.IsDeleted = true;
            load.UpdatedAt = DateTime.UtcNow;
            // await _loadRepository.DeleteAsync(loadId); // If hard delete
            await _loadRepository.UpdateAsync(load); // Assuming UpdateAsync handles IsDeleted flag for soft delete
            await LogAction(userId, AuditActionType.Delete, "Load", loadId, $"Load ID {loadId} (Job ID {jobId}) soft deleted.", user.Role);
        }

        public async Task<IEnumerable<Load>> GetLoadsForJobAsync(int jobId, int requestingUserId, UserRole requestingUserRole)
        {
            var job = await GetJobByIdAsync(jobId, requestingUserId, requestingUserRole); // This handles security
            if (job == null) return Enumerable.Empty<Load>(); // Or throw not found

            return await _loadRepository.GetLoadsByJobIdAsync(jobId); // Assumes GetLoadsByJobIdAsync filters IsDeleted=false
        }

        public async Task<Load> GetLoadByIdAsync(int loadId, int requestingUserId, UserRole requestingUserRole)
        {
            var load = await _loadRepository.GetByIdAsync(loadId);
            if (load == null || load.IsDeleted) return null;

            // Verify user has access to the job this load belongs to
            await GetJobByIdAsync(load.JobID, requestingUserId, requestingUserRole); // This will throw UnauthorizedAccessException if not allowed

            return load;
        }
    }
}
