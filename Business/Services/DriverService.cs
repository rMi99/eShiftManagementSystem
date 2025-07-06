using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Services
{
    /// <summary>
    /// Service class for driver business operations
    /// </summary>
    public class DriverService : IDriverService
    {
        private readonly DriverRepository _driverRepository;

        public DriverService()
        {
            _driverRepository = new DriverRepository();
        }

        public DriverService(DriverRepository driverRepository)
        {
            _driverRepository = driverRepository ?? throw new ArgumentNullException(nameof(driverRepository));
        }

        public Driver? GetDriverById(int driverId)
        {
            try
            {
                Logger.LogInfo($"Retrieving driver with ID: {driverId}");
                return _driverRepository.GetDriverById(driverId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error retrieving driver with ID: {driverId}");
                throw;
            }
        }

        public Driver? GetDriverByUserId(int userId)
        {
            try
            {
                Logger.LogInfo($"Retrieving driver with User ID: {userId}");
                return _driverRepository.GetDriverByUserId(userId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error retrieving driver with User ID: {userId}");
                throw;
            }
        }

        public List<Driver> GetAllDrivers()
        {
            try
            {
                Logger.LogInfo("Retrieving all drivers");
                return _driverRepository.GetAllDrivers();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error retrieving all drivers");
                throw;
            }
        }

        public List<Driver> GetAvailableDrivers()
        {
            try
            {
                Logger.LogInfo("Retrieving available drivers");
                return _driverRepository.GetActiveDrivers();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error retrieving available drivers");
                throw;
            }
        }

        public List<Driver> GetDriversByStatus(string status)
        {
            ArgumentNullException.ThrowIfNull(status);
            
            try
            {
                Logger.LogInfo($"Retrieving drivers with status: {status}");
                return _driverRepository.GetDriversByStatus(status);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error retrieving drivers with status: {status}");
                throw;
            }
        }

        public int AddDriver(Driver driver)
        {
            ArgumentNullException.ThrowIfNull(driver);

            try
            {
                ValidateDriver(driver);
                Logger.LogInfo($"Adding new driver: {driver.FullName}");
                return _driverRepository.AddDriver(driver);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error adding driver: {driver?.FullName}");
                throw;
            }
        }

        public void UpdateDriver(Driver driver)
        {
            ArgumentNullException.ThrowIfNull(driver);

            try
            {
                ValidateDriver(driver);
                Logger.LogInfo($"Updating driver: {driver.FullName}");
                _driverRepository.UpdateDriver(driver);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error updating driver: {driver?.FullName}");
                throw;
            }
        }

        public void UpdateDriverStatus(int driverId, string status)
        {
            ArgumentNullException.ThrowIfNull(status);

            try
            {
                Logger.LogInfo($"Updating driver {driverId} status to: {status}");
                _driverRepository.UpdateDriverStatus(driverId, status);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error updating driver {driverId} status to: {status}");
                throw;
            }
        }

        public void AssignVehicleToDriver(int driverId, string vehicleAssignment)
        {
            ArgumentNullException.ThrowIfNull(vehicleAssignment);

            try
            {
                Logger.LogInfo($"Assigning vehicle {vehicleAssignment} to driver {driverId}");
                _driverRepository.AssignVehicleToDriver(driverId, vehicleAssignment);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error assigning vehicle {vehicleAssignment} to driver {driverId}");
                throw;
            }
        }

        public bool IsDriverAvailable(int driverId)
        {
            try
            {
                return _driverRepository.IsDriverAvailable(driverId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error checking driver {driverId} availability");
                throw;
            }
        }

        public void DeactivateDriver(int driverId)
        {
            try
            {
                Logger.LogInfo($"Deactivating driver {driverId}");
                _driverRepository.DeleteDriver(driverId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"Error deactivating driver {driverId}");
                throw;
            }
        }

        private static void ValidateDriver(Driver driver)
        {
            if (!ValidationHelper.HasValue(driver.FirstName))
                throw new ArgumentException("First name is required.");

            if (!ValidationHelper.HasValue(driver.LastName))
                throw new ArgumentException("Last name is required.");

            if (!ValidationHelper.HasValue(driver.Phone))
                throw new ArgumentException("Phone number is required.");

            if (!ValidationHelper.IsValidPhoneNumber(driver.Phone))
                throw new ArgumentException("Invalid phone number format.");

            if (!ValidationHelper.HasValue(driver.LicenseNumber))
                throw new ArgumentException("License number is required.");

            if (driver.LicenseExpiryDate <= DateTime.Now)
                throw new ArgumentException("License expiry date must be in the future.");
        }
    }
}