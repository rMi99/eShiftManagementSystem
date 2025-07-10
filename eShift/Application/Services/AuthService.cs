using eShift.Application.Interfaces;
using eShift.DataAccess.Interfaces; // For IUserRepository, IAuditLogRepository
using eShift.Domain; // For User, AuditLog, AuditActionType
using System;
using System.Threading.Tasks;

namespace eShift.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogRepository _auditLogRepository; // For logging login/logout attempts

        public AuthService(IUserRepository userRepository, IAuditLogRepository auditLogRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _auditLogRepository = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                // Optionally log this attempt or handle as a bad request
                return null;
            }

            var user = await _userRepository.ValidateUserAsync(username, password);

            if (user != null)
            {
                // Log successful login
                var log = new AuditLog
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    ActionType = AuditActionType.Login,
                    Timestamp = DateTime.UtcNow,
                    EntityName = "User",
                    EntityID = user.UserID,
                    Changes = "User logged in successfully.",
                    AffectedRole = user.Role.ToString()
                    // IPAddress could be captured from HttpContext if this were a web app,
                    // or passed down from the presentation layer if available.
                };
                await _auditLogRepository.AddAsync(log);
                return user;
            }
            else
            {
                // Log failed login attempt (optional, be careful about logging too much info for failed attempts)
                // Consider logging without username if privacy is a concern for failed attempts
                var log = new AuditLog
                {
                    Username = username, // Log the attempted username
                    ActionType = AuditActionType.Login, // Could be a specific "LoginFailed" type
                    Timestamp = DateTime.UtcNow,
                    EntityName = "System",
                    Changes = $"Failed login attempt for username: {username}.",
                    // IPAddress = ...
                };
                await _auditLogRepository.AddAsync(log);
                return null;
            }
        }

        public async Task LogoutAsync(User user)
        {
            if (user == null) return;

            // Log logout
            var log = new AuditLog
            {
                UserID = user.UserID,
                Username = user.Username,
                ActionType = AuditActionType.Logout,
                Timestamp = DateTime.UtcNow,
                EntityName = "User",
                EntityID = user.UserID,
                Changes = "User logged out.",
                AffectedRole = user.Role.ToString()
            };
            await _auditLogRepository.AddAsync(log);

            // Any other logout specific logic can go here (e.g., invalidating a session token if using them)
        }
    }
}
