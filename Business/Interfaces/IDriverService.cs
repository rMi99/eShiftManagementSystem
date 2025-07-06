using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Interfaces
{
    /// <summary>
    /// Interface for driver business operations
    /// </summary>
    public interface IDriverService
    {
        Driver? GetDriverById(int driverId);
        Driver? GetDriverByUserId(int userId);
        List<Driver> GetAllDrivers();
        List<Driver> GetAvailableDrivers();
        List<Driver> GetDriversByStatus(string status);
        int AddDriver(Driver driver);
        void UpdateDriver(Driver driver);
        void UpdateDriverStatus(int driverId, string status);
        void AssignVehicleToDriver(int driverId, string vehicleAssignment);
        bool IsDriverAvailable(int driverId);
        void DeactivateDriver(int driverId);
    }
}