using eShift.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace eShift.DataAccess.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
    }
}
