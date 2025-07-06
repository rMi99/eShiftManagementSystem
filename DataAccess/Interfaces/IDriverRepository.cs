using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IDriverRepository
    {
        // Basic CRUD Operations
        Task<Staff?> GetDriverByIdAsync(int driverId);
        Task<Staff?> GetDriverByUserIdAsync(int userId);
        Task<List<Staff>> GetAllDriversAsync();
        Task<int> CreateDriverAsync(Staff driver);
        Task UpdateDriverAsync(Staff driver);
        Task DeleteDriverAsync(int driverId);
        Task<bool> DriverExistsAsync(int driverId);

        // Driver-Specific Queries
        Task<List<Staff>> GetActiveDriversAsync();
        Task<List<Staff>> GetAvailableDriversAsync();
        Task<List<Staff>> GetDriversByPositionAsync(string position);
        Task<Staff?> GetDriverByPhoneAsync(string phone);
        Task<List<Staff>> SearchDriversAsync(string searchTerm);

        // Assignment and Availability Management
        Task<bool> IsDriverAvailableAsync(int driverId, DateTime startDate, DateTime endDate);
        Task<List<Staff>> GetDriversAvailableForDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AssignDriverToJobAsync(int driverId, int transportUnitId);
        Task UnassignDriverFromJobAsync(int driverId, int transportUnitId);

        // Statistics and Reporting
        Task<int> GetTotalActiveDriversAsync();
        Task<int> GetTotalDriversAsync();
        Task<List<Staff>> GetDriversHiredInPeriodAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetAverageSalaryAsync();
        Task<Staff?> GetDriverWithHighestSalaryAsync();
        Task<Staff?> GetDriverWithLowestSalaryAsync();

        // Job History and Tracking
        Task<List<TransportUnit>> GetDriverJobHistoryAsync(int driverId);
        Task<int> GetDriverJobCountAsync(int driverId);
        Task<TransportUnit?> GetDriverCurrentJobAsync(int driverId);

        // Validation Methods
        Task<bool> IsPhoneNumberUniqueAsync(string phone, int? excludeDriverId = null);
        Task<bool> IsUserIdUniqueAsync(int userId, int? excludeDriverId = null);

        // Synchronous versions for compatibility with existing patterns
        Staff? GetDriverById(int driverId);
        Staff? GetDriverByUserId(int userId);
        List<Staff> GetAllDrivers();
        int CreateDriver(Staff driver);
        void UpdateDriver(Staff driver);
        void DeleteDriver(int driverId);
        bool DriverExists(int driverId);
        List<Staff> GetActiveDrivers();
        List<Staff> GetAvailableDrivers();
        List<Staff> GetDriversByPosition(string position);
        Staff? GetDriverByPhone(string phone);
        List<Staff> SearchDrivers(string searchTerm);
        bool IsDriverAvailable(int driverId, DateTime startDate, DateTime endDate);
        List<Staff> GetDriversAvailableForDateRange(DateTime startDate, DateTime endDate);
        void AssignDriverToJob(int driverId, int transportUnitId);
        void UnassignDriverFromJob(int driverId, int transportUnitId);
        int GetTotalActiveDrivers();
        int GetTotalDrivers();
        List<Staff> GetDriversHiredInPeriod(DateTime startDate, DateTime endDate);
        decimal GetAverageSalary();
        Staff? GetDriverWithHighestSalary();
        Staff? GetDriverWithLowestSalary();
        List<TransportUnit> GetDriverJobHistory(int driverId);
        int GetDriverJobCount(int driverId);
        TransportUnit? GetDriverCurrentJob(int driverId);
        bool IsPhoneNumberUnique(string phone, int? excludeDriverId = null);
        bool IsUserIdUnique(int userId, int? excludeDriverId = null);
    }
}