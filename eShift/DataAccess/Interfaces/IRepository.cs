using System.Collections.Generic;
using System.Threading.Tasks; // Assuming async operations might be useful

namespace eShift.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id); // Or pass entity, depends on preference
    }
}
