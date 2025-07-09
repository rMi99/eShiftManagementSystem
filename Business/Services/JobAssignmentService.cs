using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eShiftManagementSystem.Business.Services
{
    public class JobAssignmentService
    {
        private readonly JobRepository _jobRepository;
        private readonly DriverRepository _driverRepository;
        private readonly AssistantRepository _assistantRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly ContainerRepository _containerRepository;
        private readonly AuditService _auditService;

        public JobAssignmentService()
        {
            _jobRepository = new JobRepository();
            _driverRepository = new DriverRepository();
            _assistantRepository = new AssistantRepository();
            _vehicleRepository = new VehicleRepository();
            _containerRepository = new ContainerRepository();
            _auditService = new AuditService();
        }

        public class JobAssignmentResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public TransportUnit? AssignedTransportUnit { get; set; }
        }

        public class ResourceRequirements
        {
            public decimal EstimatedWeight { get; set; }
            public decimal EstimatedVolume { get; set; }
            public bool RequiresSpecialHandling { get; set; }
            public bool IsFragile { get; set; }
            public string? DriverPreference { get; set; }
            public string? VehicleTypePreference { get; set; }
        }

        public JobAssignmentResult AssignResourcesAutomatic(int jobId, ResourceRequirements requirements, int assignedBy)
        {
            try
            {
                var job = _jobRepository.GetJobById(jobId);
                if (job == null)
                {
                    return new JobAssignmentResult { Success = false, Message = "Job not found." };
                }

                if (job.Status != "pending" && job.Status != "accepted")
                {
                    return new JobAssignmentResult { Success = false, Message = "Job is not in a state that allows resource assignment." };
                }

                // Find suitable vehicle
                var suitableVehicles = GetSuitableVehicles(requirements);
                if (!suitableVehicles.Any())
                {
                    return new JobAssignmentResult { Success = false, Message = "No suitable vehicles available for this job." };
                }

                // Find available driver
                var availableDrivers = _driverRepository.GetAvailableDrivers();
                var suitableDriver = GetBestDriver(availableDrivers, requirements);
                if (suitableDriver == null)
                {
                    return new JobAssignmentResult { Success = false, Message = "No suitable drivers available for this job." };
                }

                // Find available assistant if required
                Assistant? suitableAssistant = null;
                if (requirements.RequiresSpecialHandling || requirements.IsFragile)
                {
                    var availableAssistants = _assistantRepository.GetAvailableAssistants();
                    suitableAssistant = GetBestAssistant(availableAssistants, requirements);
                }

                // Find suitable container if needed
                Container? suitableContainer = null;
                if (requirements.EstimatedVolume > 10) // Large jobs need containers
                {
                    var availableContainers = _containerRepository.GetAvailableContainers();
                    suitableContainer = GetBestContainer(availableContainers, requirements);
                }

                // Create transport unit and assign to job
                var transportUnit = CreateTransportUnit(suitableVehicles.First(), suitableDriver, suitableAssistant, suitableContainer);
                var result = AssignTransportUnitToJob(jobId, transportUnit, assignedBy);

                if (result.Success)
                {
                    // Update availability status
                    _driverRepository.UpdateDriverAvailability(suitableDriver.DriverId, false);
                    if (suitableAssistant != null)
                    {
                        _assistantRepository.UpdateAssistantAvailability(suitableAssistant.AssistantId, false, jobId);
                    }

                    _auditService.LogCreate("transport_units", transportUnit.TransportUnitId, transportUnit, assignedBy);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new JobAssignmentResult { Success = false, Message = $"Error during automatic assignment: {ex.Message}" };
            }
        }

        public JobAssignmentResult AssignResourcesManual(int jobId, int? driverId, int? assistantId, 
            int? vehicleId, int? containerId, int assignedBy)
        {
            try
            {
                var job = _jobRepository.GetJobById(jobId);
                if (job == null)
                {
                    return new JobAssignmentResult { Success = false, Message = "Job not found." };
                }

                // Validate driver
                Driver? driver = null;
                if (driverId.HasValue)
                {
                    driver = _driverRepository.GetDriverById(driverId.Value);
                    if (driver == null || !driver.IsAvailable)
                    {
                        return new JobAssignmentResult { Success = false, Message = "Selected driver is not available." };
                    }
                }

                // Validate assistant
                Assistant? assistant = null;
                if (assistantId.HasValue)
                {
                    assistant = _assistantRepository.GetAssistantById(assistantId.Value);
                    if (assistant == null || !assistant.IsAvailable)
                    {
                        return new JobAssignmentResult { Success = false, Message = "Selected assistant is not available." };
                    }
                }

                // Validate vehicle
                Vehicle? vehicle = null;
                if (vehicleId.HasValue)
                {
                    vehicle = _vehicleRepository.GetVehicleById(vehicleId.Value);
                    if (vehicle == null || !vehicle.IsAvailable)
                    {
                        return new JobAssignmentResult { Success = false, Message = "Selected vehicle is not available." };
                    }
                }

                // Validate container
                Container? container = null;
                if (containerId.HasValue)
                {
                    container = _containerRepository.GetContainerById(containerId.Value);
                    if (container == null || !container.IsAvailable)
                    {
                        return new JobAssignmentResult { Success = false, Message = "Selected container is not available." };
                    }
                }

                if (vehicle == null)
                {
                    return new JobAssignmentResult { Success = false, Message = "A vehicle must be assigned to the job." };
                }

                // Create transport unit and assign
                var transportUnit = CreateTransportUnit(vehicle, driver, assistant, container);
                var result = AssignTransportUnitToJob(jobId, transportUnit, assignedBy);

                if (result.Success)
                {
                    // Update availability status
                    if (driver != null)
                        _driverRepository.UpdateDriverAvailability(driver.DriverId, false);
                    if (assistant != null)
                        _assistantRepository.UpdateAssistantAvailability(assistant.AssistantId, false, jobId);

                    _auditService.LogCreate("transport_units", transportUnit.TransportUnitId, transportUnit, assignedBy);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new JobAssignmentResult { Success = false, Message = $"Error during manual assignment: {ex.Message}" };
            }
        }

        private List<Vehicle> GetSuitableVehicles(ResourceRequirements requirements)
        {
            var availableVehicles = _vehicleRepository.GetAvailableVehicles();
            
            return availableVehicles.Where(v => 
                (requirements.EstimatedWeight == 0 || v.MaxWeight >= requirements.EstimatedWeight) &&
                (requirements.EstimatedVolume == 0 || v.MaxVolume >= requirements.EstimatedVolume))
                .OrderBy(v => v.MaxWeight) // Prefer smaller suitable vehicles
                .ToList();
        }

        private Driver? GetBestDriver(List<Driver> availableDrivers, ResourceRequirements requirements)
        {
            var suitableDrivers = availableDrivers.Where(d => d.IsLicenseValid);

            if (!string.IsNullOrEmpty(requirements.DriverPreference))
            {
                var preferredDriver = suitableDrivers.FirstOrDefault(d => 
                    d.FullName.Contains(requirements.DriverPreference, StringComparison.OrdinalIgnoreCase));
                if (preferredDriver != null) return preferredDriver;
            }

            if (requirements.RequiresSpecialHandling)
            {
                // Prefer experienced drivers for special handling
                var experiencedDrivers = suitableDrivers.Where(d => d.IsExperienced);
                if (experiencedDrivers.Any())
                {
                    return experiencedDrivers.OrderByDescending(d => d.Rating).First();
                }
            }

            return suitableDrivers.OrderByDescending(d => d.Rating).FirstOrDefault();
        }

        private Assistant? GetBestAssistant(List<Assistant> availableAssistants, ResourceRequirements requirements)
        {
            if (requirements.IsFragile)
            {
                var fragileHandlers = availableAssistants.Where(a => 
                    a.Specialization?.Contains("fragile", StringComparison.OrdinalIgnoreCase) == true);
                if (fragileHandlers.Any())
                {
                    return fragileHandlers.OrderByDescending(a => a.Rating).First();
                }
            }

            return availableAssistants.OrderByDescending(a => a.Rating).FirstOrDefault();
        }

        private Container? GetBestContainer(List<Container> availableContainers, ResourceRequirements requirements)
        {
            return availableContainers.Where(c => 
                c.MaxWeight >= requirements.EstimatedWeight &&
                c.MaxVolume >= requirements.EstimatedVolume)
                .OrderBy(c => c.MaxWeight) // Prefer smaller suitable containers
                .FirstOrDefault();
        }

        private TransportUnit CreateTransportUnit(Vehicle vehicle, Driver? driver, Assistant? assistant, Container? container)
        {
            var transportUnit = new TransportUnit
            {
                UnitName = $"TU-{DateTime.Now:yyyyMMddHHmmss}",
                VehicleId = vehicle.VehicleId,
                DriverId = driver?.DriverId,
                AssistantId = assistant?.AssistantId,
                ContainerId = container?.ContainerId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // This would need a TransportUnitRepository to save
            // For now, return the object with a generated ID
            transportUnit.TransportUnitId = new Random().Next(1000, 9999); // Temporary ID

            return transportUnit;
        }

        private JobAssignmentResult AssignTransportUnitToJob(int jobId, TransportUnit transportUnit, int assignedBy)
        {
            try
            {
                var job = _jobRepository.GetJobById(jobId);
                if (job == null)
                {
                    return new JobAssignmentResult { Success = false, Message = "Job not found." };
                }

                // Update job with transport unit
                var oldJob = job; // For audit trail
                job.Status = "assigned";
                job.UpdatedAt = DateTime.Now;
                // job.TransportUnitId = transportUnit.TransportUnitId; // This property needs to be added to Job model

                _jobRepository.UpdateJob(job);
                _auditService.LogUpdate("jobs", jobId, oldJob, job, assignedBy);

                return new JobAssignmentResult 
                { 
                    Success = true, 
                    Message = "Resources assigned successfully.", 
                    AssignedTransportUnit = transportUnit 
                };
            }
            catch (Exception ex)
            {
                return new JobAssignmentResult { Success = false, Message = $"Error assigning transport unit: {ex.Message}" };
            }
        }

        public void ReleaseJobResources(int jobId, int releasedBy)
        {
            try
            {
                var job = _jobRepository.GetJobById(jobId);
                if (job == null) return;

                // This would need to get transport unit info from job
                // and update driver/assistant availability back to true
                
                var oldJob = job;
                job.Status = "completed";
                job.UpdatedAt = DateTime.Now;
                
                _jobRepository.UpdateJob(job);
                _auditService.LogUpdate("jobs", jobId, oldJob, job, releasedBy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error releasing job resources: {ex.Message}");
            }
        }
    }
}