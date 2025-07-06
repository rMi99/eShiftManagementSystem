using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IDriverRepository
    {
        Driver? GetDriverById(int driverId);
        Driver? GetDriverByUserId(int userId);
        List<Driver> GetAllDrivers();
        List<Driver> GetActiveDrivers();
        List<Driver> GetDriversByStatus(string status);
        int AddDriver(Driver driver);
        void UpdateDriver(Driver driver);
        void UpdateDriverStatus(int driverId, string status);
        void DeleteDriver(int driverId);
        bool DriverExists(int driverId);
        bool IsDriverAvailable(int driverId);
        void AssignVehicleToDriver(int driverId, string vehicleAssignment);
    }
}