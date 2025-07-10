using eShift.Domain; // Required for User entity
using System.Threading.Tasks;

namespace eShift.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> ValidateUserAsync(string username, string password); // Password here is the plain text one for validation against stored hash
        // Add other user-specific methods if needed, e.g., finding users by role
    }
}
