using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> GetAllVehicles();
        Vehicle GetVehicleById(int id);
        void CreateVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        void DeleteVehicle(int id);
    }
}