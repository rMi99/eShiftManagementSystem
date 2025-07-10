using eShift.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace eShift.DataAccess.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(string type);
        Task<IEnumerable<Vehicle>> GetVehiclesByStatusAsync(VehicleStatus status);
    }
}
