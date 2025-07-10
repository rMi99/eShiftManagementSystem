using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IStaffRepository
    {
        Staff? GetStaffById(int staffId);
        List<Staff> GetAllStaff();
        List<Staff> GetAllDrivers();
        int AddStaff(Staff staff);
        void UpdateStaff(Staff staff);
        void DeleteStaff(int staffId);
    }
}