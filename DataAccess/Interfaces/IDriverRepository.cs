using eShiftManagementSystem.Models;
using System.Collections.Generic;
using System;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IDriverRepository
    {
        // Basic CRUD Operations
        List<Staff> GetAllDrivers();
        Staff? GetDriverById(int driverId);
        Staff? GetDriverByUserId(int userId);
        int CreateDriver(Staff driver);
        void UpdateDriver(Staff driver);
        void DeleteDriver(int driverId);
        bool DriverExists(int driverId);

        // Driver-Specific Queries
        List<Staff> GetActiveDrivers();
        List<Staff> GetAvailableDrivers();
        List<Staff> GetDriversByPosition(string position);
        Staff? GetDriverByPhone(string phone);
        List<Staff> SearchDrivers(string searchTerm);

        // Assignment and Availability Management
        bool IsDriverAvailable(int driverId, DateTime startDate, DateTime endDate);
        List<Staff> GetDriversAvailableForDateRange(DateTime startDate, DateTime endDate);
        void AssignDriverToJob(int driverId, int jobId);
        void UnassignDriverFromJob(int driverId, int jobId);

        // Statistics and Reporting
        int GetTotalActiveDrivers();
        int GetTotalDrivers();
        List<Staff> GetDriversHiredInPeriod(DateTime startDate, DateTime endDate);
        decimal GetAverageSalary();
        Staff? GetDriverWithHighestSalary();
        Staff? GetDriverWithLowestSalary();

        // Job History and Tracking
        List<TransportUnit> GetDriverJobHistory(int driverId);
        int GetDriverJobCount(int driverId);
        TransportUnit? GetDriverCurrentJob(int driverId);

        // Validation Methods
        bool IsPhoneNumberUnique(string phone, int? excludeDriverId = null);
        bool IsUserIdUnique(int userId, int? excludeDriverId = null);
    }
}