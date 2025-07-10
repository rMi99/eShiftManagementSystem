using Dapper;
using eShift.DataAccess.Interfaces;
using eShift.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
// It's good practice to have a password hashing library.
// For this example, I'll assume a utility class or direct implementation for hashing.
// using System.Security.Cryptography;
// using System.Text;

namespace eShift.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // A simple password hashing placeholder. In a real app, use a strong library like BCrypt.Net or ASP.NET Core Identity's hasher.
        private string HashPassword(string password)
        {
            // IMPORTANT: This is a placeholder and NOT secure for production.
            // Use a proper password hashing library (e.g., BCrypt.Net, PBKDF2).
            // using (var sha256 = System.Security.Cryptography.SHA256.Create())
            // {
            //    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            //    return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            // }
            return "hashed_" + password; // Placeholder
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // IMPORTANT: This is a placeholder and NOT secure for production.
            // Implement proper verification against the stored hash.
            return storedHash == "hashed_" + enteredPassword; // Placeholder
        }


        public async Task AddAsync(User entity)
        {
            entity.PasswordHash = HashPassword(entity.PasswordHash); // Assuming PasswordHash temporarily holds plain password
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            var sql = @"INSERT INTO Users (Username, PasswordHash, Role, CustomerID, IsActive, CreatedAt, UpdatedAt)
                        VALUES (@Username, @PasswordHash, @Role, @CustomerID, @IsActive, @CreatedAt, @UpdatedAt);
                        SELECT LAST_INSERT_ID();"; // For MySQL to get the last inserted ID

            using (var connection = DbConnectionFactory.CreateConnection())
            {
                entity.UserID = await connection.ExecuteScalarAsync<int>(sql, entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            // Consider soft delete: UPDATE Users SET IsActive = 0 WHERE UserID = @Id
            var sql = "UPDATE Users SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE UserID = @Id";
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.UtcNow });
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users WHERE IsActive = 1";
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<User>(sql);
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE UserID = @Id AND IsActive = 1";
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username AND IsActive = 1";
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
            }
        }

        public async Task UpdateAsync(User entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            // Password update should be handled carefully, e.g., through a separate method
            // and only if a new password is provided.
            // For this generic update, we assume PasswordHash is not changed or already hashed if changed.

            var sql = @"UPDATE Users
                        SET Username = @Username,
                            Role = @Role,
                            CustomerID = @CustomerID,
                            IsActive = @IsActive,
                            UpdatedAt = @UpdatedAt
                        WHERE UserID = @UserID;";
            // If password change is allowed:
            // PasswordHash = @PasswordHash, (ensure it's hashed before this call)

            using (var connection = DbConnectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);
            if (user != null)
            {
                // In a real app, use a secure password verification method.
                if (VerifyPassword(password, user.PasswordHash))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
