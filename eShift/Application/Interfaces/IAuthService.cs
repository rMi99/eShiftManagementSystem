using eShift.Domain; // For User entity
using System.Threading.Tasks;

namespace eShift.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string username, string password);
        Task LogoutAsync(User user); // Placeholder for any logout specific logic (e.g. audit logging)
        // Potentially: Task RegisterCustomerAsync(Customer customer, string username, string password);
    }
}
