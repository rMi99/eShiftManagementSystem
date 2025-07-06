using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Services
{
    public class JobService : IJobService
    {
        private readonly JobRepository _jobRepository;

        public JobService()
        {
            _jobRepository = new JobRepository();
        }

        public List<Job> GetAllJobs()
        {
            try
            {
                return _jobRepository.GetAllJobs();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving jobs: {ex.Message}");
            }
        }

        public Job? GetJobById(int jobId)
        {
            try
            {
                return _jobRepository.GetJobById(jobId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving job: {ex.Message}");
            }
        }

        public List<Job> GetJobsByCustomerId(int customerId)
        {
            try
            {
                return _jobRepository.GetJobsByCustomerId(customerId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving jobs for customer: {ex.Message}");
            }
        }

        public List<Job> GetJobsByStatus(string status)
        {
            try
            {
                return _jobRepository.GetJobsByStatus(status);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving jobs by status: {ex.Message}");
            }
        }

        public int CreateJob(Job job)
        {
            try
            {
                if (job.CustomerId <= 0)
                {
                    throw new ArgumentException("Valid customer ID is required.");
                }

                if (string.IsNullOrWhiteSpace(job.PickupAddress) || string.IsNullOrWhiteSpace(job.DestinationAddress))
                {
                    throw new ArgumentException("Pickup and destination addresses are required.");
                }

                // Generate job number if not provided
                if (string.IsNullOrWhiteSpace(job.JobNumber))
                {
                    job.JobNumber = GenerateJobNumber();
                }

                job.CreatedAt = DateTime.Now;
                job.UpdatedAt = DateTime.Now;
                job.Status = "pending";

                return _jobRepository.AddJob(job);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating job: {ex.Message}");
            }
        }

        public void UpdateJob(Job job)
        {
            try
            {
                if (job.JobId <= 0)
                {
                    throw new ArgumentException("Invalid job ID.");
                }

                job.UpdatedAt = DateTime.Now;
                _jobRepository.UpdateJob(job);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating job: {ex.Message}");
            }
        }

        public void DeleteJob(int jobId)
        {
            try
            {
                if (jobId <= 0)
                {
                    throw new ArgumentException("Invalid job ID.");
                }

                _jobRepository.DeleteJob(jobId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting job: {ex.Message}");
            }
        }

        public void UpdateJobStatus(int jobId, string status)
        {
            try
            {
                if (jobId <= 0)
                {
                    throw new ArgumentException("Invalid job ID.");
                }

                if (string.IsNullOrWhiteSpace(status))
                {
                    throw new ArgumentException("Status is required.");
                }

                _jobRepository.UpdateJobStatus(jobId, status);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating job status: {ex.Message}");
            }
        }

        public string GenerateJobNumber()
        {
            return NumberGenerator.GenerateJobNumber();
        }
    }
}