using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Interfaces
{
    public interface IStaffService
    {
        List<Staff> GetAllStaff();
        List<Staff> GetAllDrivers();
        Staff GetStaffById(int id);
        void CreateStaff(Staff staff);
        void UpdateStaff(Staff staff);
        void DeleteStaff(int id);
    }
}