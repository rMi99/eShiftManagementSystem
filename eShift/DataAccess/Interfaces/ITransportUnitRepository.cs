using eShift.Domain;
using System.Threading.Tasks;

namespace eShift.DataAccess.Interfaces
{
    public interface ITransportUnitRepository : IRepository<TransportUnit>
    {
        Task<TransportUnit> GetByJobIdAsync(int jobId);
        // Add other transport unit-specific methods
    }
}
