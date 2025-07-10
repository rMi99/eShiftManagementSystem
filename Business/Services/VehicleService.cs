using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleRepository _vehicleRepository;

        public VehicleService()
        {
            _vehicleRepository = new VehicleRepository();
        }

        public List<Vehicle> GetAllVehicles()
        {
            return _vehicleRepository.GetAllVehicles();
        }

        public Vehicle GetVehicleById(int id)
        {
            return _vehicleRepository.GetVehicleById(id);
        }

        public void CreateVehicle(Vehicle vehicle)
        {
            _vehicleRepository.AddVehicle(vehicle);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            _vehicleRepository.UpdateVehicle(vehicle);
        }

        public void DeleteVehicle(int id)
        {
            _vehicleRepository.DeleteVehicle(id);
        }
    }
}