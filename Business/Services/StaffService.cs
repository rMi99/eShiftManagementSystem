using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Services
{
    public class StaffService : IStaffService
    {
        private readonly StaffRepository _staffRepository;

        public StaffService()
        {
            _staffRepository = new StaffRepository();
        }

        public List<Staff> GetAllStaff()
        {
            return _staffRepository.GetAllStaff();
        }

        public List<Staff> GetAllDrivers()
        {
            return _staffRepository.GetAllDrivers();
        }

        public Staff GetStaffById(int id)
        {
            return _staffRepository.GetStaffById(id);
        }

        public void CreateStaff(Staff staff)
        {
            _staffRepository.AddStaff(staff);
        }

        public void UpdateStaff(Staff staff)
        {
            _staffRepository.UpdateStaff(staff);
        }

        public void DeleteStaff(int id)
        {
            _staffRepository.DeleteStaff(id);
        }
    }
}