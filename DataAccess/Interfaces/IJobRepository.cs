using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IJobRepository
    {
        Job? GetJobById(int jobId);
        List<Job> GetAllJobs();
        List<Job> GetJobsByCustomerId(int customerId);
        List<Job> GetJobsByStatus(string status);
        List<Job> GetJobsByDateRange(DateTime startDate, DateTime endDate);
        List<Job> SearchJobs(string searchTerm);
        int AddJob(Job job);
        void UpdateJob(Job job);
        void UpdateJobStatus(int jobId, string status);
        void DeleteJob(int jobId);
        bool JobExists(string jobNumber);
        int GetJobCountByStatus(string status);
        List<Job> GetRecentJobs(int count);
        List<Job> GetJobsForDriver(int driverId);
        List<Job> GetJobsByDriverId(int driverId);  // Alias for GetJobsForDriver
        void AssignJobToDriver(int jobId, int driverId);
        void UpdateJobProgress(int jobId, string progress);
    }
}