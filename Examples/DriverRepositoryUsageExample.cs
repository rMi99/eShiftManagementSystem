using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Examples
{
    /// <summary>
    /// Example usage of the DriverRepository implementation
    /// This class demonstrates how to use the comprehensive driver repository
    /// in various scenarios within the e-Shift Management System
    /// </summary>
    public class DriverRepositoryUsageExample
    {
        private readonly DriverRepository _driverRepository;

        public DriverRepositoryUsageExample()
        {
            _driverRepository = new DriverRepository();
        }

        /// <summary>
        /// Example: Creating a new driver
        /// </summary>
        public void CreateDriverExample()
        {
            try
            {
                var newDriver = new Staff
                {
                    UserId = 1001,
                    FirstName = "John",
                    LastName = "Smith",
                    Phone = "555-0123",
                    Address = "123 Main St, City, State",
                    Position = "Delivery Driver",
                    HireDate = DateTime.Now.Date,
                    Salary = 45000,
                    IsActive = true
                };

                // Validate phone number is unique before creating
                if (!_driverRepository.IsPhoneNumberUnique(newDriver.Phone))
                {
                    throw new Exception("Phone number already exists");
                }

                // Validate user ID is unique
                if (!_driverRepository.IsUserIdUnique(newDriver.UserId))
                {
                    throw new Exception("User ID already assigned to another staff member");
                }

                int driverId = _driverRepository.CreateDriver(newDriver);
                Console.WriteLine($"Driver created successfully with ID: {driverId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating driver: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Getting available drivers for job assignment
        /// </summary>
        public void GetAvailableDriversExample()
        {
            try
            {
                // Get all available drivers (not currently assigned)
                var availableDrivers = _driverRepository.GetAvailableDrivers();
                Console.WriteLine($"Found {availableDrivers.Count} available drivers");

                // Get drivers available for specific date range
                var startDate = DateTime.Today.AddDays(1);
                var endDate = DateTime.Today.AddDays(7);
                var driversForDateRange = _driverRepository.GetDriversAvailableForDateRange(startDate, endDate);
                Console.WriteLine($"Found {driversForDateRange.Count} drivers available from {startDate:MM/dd/yyyy} to {endDate:MM/dd/yyyy}");

                foreach (var driver in driversForDateRange)
                {
                    Console.WriteLine($"- {driver.FullName} (ID: {driver.StaffId}, Position: {driver.Position})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving available drivers: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Assigning a driver to a job
        /// </summary>
        public void AssignDriverToJobExample(int driverId, int jobId)
        {
            try
            {
                // Check if driver exists and is active
                var driver = _driverRepository.GetDriverById(driverId);
                if (driver == null)
                {
                    throw new Exception("Driver not found");
                }

                if (!driver.IsActive)
                {
                    throw new Exception("Cannot assign inactive driver to job");
                }

                // Check if driver is available for the job dates
                var startDate = DateTime.Today;
                var endDate = DateTime.Today.AddDays(3);
                if (!_driverRepository.IsDriverAvailable(driverId, startDate, endDate))
                {
                    throw new Exception("Driver is not available for the specified dates");
                }

                // Assign driver to job
                _driverRepository.AssignDriverToJob(driverId, jobId);
                Console.WriteLine($"Driver {driver.FullName} successfully assigned to job {jobId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning driver to job: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Getting driver statistics and reports
        /// </summary>
        public void GetDriverStatisticsExample()
        {
            try
            {
                // Get overall statistics
                int totalDrivers = _driverRepository.GetTotalDrivers();
                int activeDrivers = _driverRepository.GetTotalActiveDrivers();
                decimal averageSalary = _driverRepository.GetAverageSalary();

                Console.WriteLine("=== Driver Statistics ===");
                Console.WriteLine($"Total Drivers: {totalDrivers}");
                Console.WriteLine($"Active Drivers: {activeDrivers}");
                Console.WriteLine($"Average Salary: ${averageSalary:F2}");

                // Get highest and lowest paid drivers
                var highestPaid = _driverRepository.GetDriverWithHighestSalary();
                var lowestPaid = _driverRepository.GetDriverWithLowestSalary();

                if (highestPaid != null)
                {
                    Console.WriteLine($"Highest Paid: {highestPaid.FullName} - ${highestPaid.Salary:F2}");
                }

                if (lowestPaid != null)
                {
                    Console.WriteLine($"Lowest Paid: {lowestPaid.FullName} - ${lowestPaid.Salary:F2}");
                }

                // Get drivers hired in the last 30 days
                var recentHires = _driverRepository.GetDriversHiredInPeriod(
                    DateTime.Today.AddDays(-30), 
                    DateTime.Today);
                Console.WriteLine($"Recent Hires (last 30 days): {recentHires.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving driver statistics: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Getting driver job history and current assignments
        /// </summary>
        public void GetDriverJobHistoryExample(int driverId)
        {
            try
            {
                var driver = _driverRepository.GetDriverById(driverId);
                if (driver == null)
                {
                    Console.WriteLine("Driver not found");
                    return;
                }

                Console.WriteLine($"=== Job Information for {driver.FullName} ===");

                // Get current job
                var currentJob = _driverRepository.GetDriverCurrentJob(driverId);
                if (currentJob != null && currentJob.Job != null)
                {
                    Console.WriteLine($"Current Job: {currentJob.Job.JobNumber} - {currentJob.Job.PickupCity} to {currentJob.Job.DestinationCity}");
                    Console.WriteLine($"Status: {currentJob.Status}");
                }
                else
                {
                    Console.WriteLine("No current job assignment");
                }

                // Get job count
                int jobCount = _driverRepository.GetDriverJobCount(driverId);
                Console.WriteLine($"Total Jobs Completed: {jobCount}");

                // Get job history
                var jobHistory = _driverRepository.GetDriverJobHistory(driverId);
                Console.WriteLine($"Job History ({jobHistory.Count} jobs):");
                
                foreach (var transportUnit in jobHistory)
                {
                    if (transportUnit.Job != null)
                    {
                        Console.WriteLine($"- Job {transportUnit.Job.JobNumber}: {transportUnit.Job.PickupCity} → {transportUnit.Job.DestinationCity}");
                        Console.WriteLine($"  Date: {transportUnit.Job.RequestedPickupDate:MM/dd/yyyy}, Status: {transportUnit.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving driver job history: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Searching and filtering drivers
        /// </summary>
        public void SearchDriversExample()
        {
            try
            {
                // Search by name or phone
                var searchResults = _driverRepository.SearchDrivers("Smith");
                Console.WriteLine($"Search results for 'Smith': {searchResults.Count} drivers found");

                // Get drivers by specific position
                var deliveryDrivers = _driverRepository.GetDriversByPosition("Delivery Driver");
                Console.WriteLine($"Delivery Drivers: {deliveryDrivers.Count}");

                var truckDrivers = _driverRepository.GetDriversByPosition("Truck Driver");
                Console.WriteLine($"Truck Drivers: {truckDrivers.Count}");

                // Find driver by phone number
                var driverByPhone = _driverRepository.GetDriverByPhone("555-0123");
                if (driverByPhone != null)
                {
                    Console.WriteLine($"Found driver by phone: {driverByPhone.FullName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching drivers: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Updating driver information
        /// </summary>
        public void UpdateDriverExample(int driverId)
        {
            try
            {
                var driver = _driverRepository.GetDriverById(driverId);
                if (driver == null)
                {
                    Console.WriteLine("Driver not found");
                    return;
                }

                // Update driver information
                driver.Phone = "555-9999";
                driver.Salary = 50000;
                driver.Address = "456 New Address, City, State";

                // Validate new phone number is unique (excluding current driver)
                if (!_driverRepository.IsPhoneNumberUnique(driver.Phone, driverId))
                {
                    throw new Exception("Phone number already exists for another driver");
                }

                _driverRepository.UpdateDriver(driver);
                Console.WriteLine($"Driver {driver.FullName} updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating driver: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Integration with job management workflow
        /// </summary>
        public void JobManagementWorkflowExample()
        {
            try
            {
                Console.WriteLine("=== Job Management Workflow Example ===");

                // Step 1: Get available drivers for a new job
                var availableDrivers = _driverRepository.GetAvailableDrivers();
                Console.WriteLine($"Step 1: Found {availableDrivers.Count} available drivers");

                if (availableDrivers.Count == 0)
                {
                    Console.WriteLine("No drivers available for assignment");
                    return;
                }

                // Step 2: Select the first available driver
                var selectedDriver = availableDrivers[0];
                Console.WriteLine($"Step 2: Selected driver {selectedDriver.FullName}");

                // Step 3: Verify driver can handle the job dates
                var jobStartDate = DateTime.Today.AddDays(1);
                var jobEndDate = DateTime.Today.AddDays(2);
                
                if (_driverRepository.IsDriverAvailable(selectedDriver.StaffId, jobStartDate, jobEndDate))
                {
                    Console.WriteLine($"Step 3: Driver is available for {jobStartDate:MM/dd/yyyy} - {jobEndDate:MM/dd/yyyy}");
                    
                    // Step 4: Assign driver to job (using sample job ID)
                    int sampleJobId = 1001;
                    _driverRepository.AssignDriverToJob(selectedDriver.StaffId, sampleJobId);
                    Console.WriteLine($"Step 4: Driver assigned to job {sampleJobId}");
                }
                else
                {
                    Console.WriteLine("Step 3: Driver is not available for the specified dates");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in job management workflow: {ex.Message}");
            }
        }
    }
}