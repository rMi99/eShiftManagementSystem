using eShift.Domain; // Required for Load entity
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShift.DataAccess.Interfaces
{
    public interface ILoadRepository : IRepository<Load>
    {
        Task<IEnumerable<Load>> GetLoadsByJobIdAsync(int jobId);
        Task<IEnumerable<Load>> SearchLoadsAsync(int? jobId, bool? isFragile, string searchTerm);
        // Add other load-specific methods
    }
}
