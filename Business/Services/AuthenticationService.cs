using eShiftManagementSystem.Business.Interfaces;
using eShiftManagementSystem.DataAccess.Repositories;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace eShiftManagementSystem.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserRepository _userRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly Dictionary<string, string> _passwordResetTokens;

        public AuthenticationService()
        {
            _userRepository = new UserRepository();
            _customerRepository = new CustomerRepository();
            _passwordResetTokens = new Dictionary<string, string>();
        }

        public AuthenticationService(UserRepository userRepository, CustomerRepository customerRepository)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _passwordResetTokens = new Dictionary<string, string>();
        }

        public User? Login(string username, string password)
        {
            try
            {
                var user = _userRepository.GetUserByUsername(username);
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                    user.LastLogin = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                    SessionManager.SetCurrentUser(user);
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login error: {ex.Message}");
            }
        }

        public bool RegisterCustomer(string username, string email, string password, Customer customer)
        {
            try
            {
                // Check if username or email already exists
                if (_userRepository.GetUserByUsername(username) != null)
                    return false;

                if (_userRepository.GetUserByEmail(email) != null)
                    return false;

                // Create user account
                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    Role = "customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var userId = _userRepository.AddUser(user);

                // Create customer profile
                customer.UserId = userId;
                customer.RegistrationDate = DateTime.Now;
                _customerRepository.AddCustomer(customer);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration error: {ex.Message}");
            }
        }

        public bool RegisterUser(string username, string email, string password, string role)
        {
            try
            {
                // Check if username or email already exists
                if (!IsUsernameAvailable(username) || !IsEmailAvailable(email))
                    return false;

                // Validate password
                if (!ValidatePassword(password))
                    return false;

                // Create user account
                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    Role = role,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _userRepository.AddUser(user);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"User registration error: {ex.Message}");
            }
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                var user = _userRepository.GetUserByUsername(username);
                if (user != null && VerifyPassword(oldPassword, user.PasswordHash))
                {
                    if (!ValidatePassword(newPassword))
                        return false;

                    user.PasswordHash = HashPassword(newPassword);
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password change error: {ex.Message}");
            }
        }

        public bool ResetPassword(string email, string newPassword)
        {
            try
            {
                var user = _userRepository.GetUserByEmail(email);
                if (user != null)
                {
                    if (!ValidatePassword(newPassword))
                        return false;

                    user.PasswordHash = HashPassword(newPassword);
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password reset error: {ex.Message}");
            }
        }

        public string GeneratePasswordResetToken(string email)
        {
            try
            {
                var user = _userRepository.GetUserByEmail(email);
                if (user != null)
                {
                    var token = Guid.NewGuid().ToString();
                    _passwordResetTokens[token] = email;
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token generation error: {ex.Message}");
            }
        }

        public bool ResetPasswordWithToken(string token, string newPassword)
        {
            try
            {
                if (_passwordResetTokens.ContainsKey(token))
                {
                    var email = _passwordResetTokens[token];
                    var result = ResetPassword(email, newPassword);
                    if (result)
                    {
                        _passwordResetTokens.Remove(token);
                    }
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password reset with token error: {ex.Message}");
            }
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            // At least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // At least one lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // At least one digit
            if (!Regex.IsMatch(password, @"\d"))
                return false;

            // At least one special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':;{}|<>]"))
                return false;

            return true;
        }

        public string GetPasswordRequirements()
        {
            return "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
        }

        public bool IsUsernameAvailable(string username)
        {
            try
            {
                return _userRepository.GetUserByUsername(username) == null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Username availability check error: {ex.Message}");
            }
        }

        public bool IsEmailAvailable(string email)
        {
            try
            {
                return _userRepository.GetUserByEmail(email) == null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Email availability check error: {ex.Message}");
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                return _userRepository.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Get user by ID error: {ex.Message}");
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                return _userRepository.GetUserByUsername(username);
            }
            catch (Exception ex)
            {
                throw new Exception($"Get user by username error: {ex.Message}");
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                return _userRepository.GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Get user by email error: {ex.Message}");
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return _userRepository.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception($"Get all users error: {ex.Message}");
            }
        }

        public List<User> GetUsersByRole(string role)
        {
            try
            {
                // Since UserRepository doesn't have GetUsersByRole, we'll implement it here
                var allUsers = _userRepository.GetAllUsers();
                return allUsers.Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Get users by role error: {ex.Message}");
            }
        }

        public bool UpdateUserRole(int userId, string role)
        {
            try
            {
                var user = _userRepository.GetUserById(userId);
                if (user != null)
                {
                    user.Role = role;
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Update user role error: {ex.Message}");
            }
        }

        public bool SetUserActiveStatus(int userId, bool isActive)
        {
            try
            {
                var user = _userRepository.GetUserById(userId);
                if (user != null)
                {
                    user.IsActive = isActive;
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Set user active status error: {ex.Message}");
            }
        }

        public void UpdateLastLogin(int userId)
        {
            try
            {
                var user = _userRepository.GetUserById(userId);
                if (user != null)
                {
                    user.LastLogin = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Update last login error: {ex.Message}");
            }
        }

        public void LogUserLogout(int userId)
        {
            try
            {
                // Check if it's the current user before logging out
                if (SessionManager.IsLoggedIn() && SessionManager.GetUserId() == userId)
                {
                    SessionManager.Logout();
                }

                // You could also log the logout event here
                // For example: LogoutEventLogger.Log(userId, DateTime.Now);
            }
            catch (Exception ex)
            {
                throw new Exception($"Log user logout error: {ex.Message}");
            }
        }

        public bool IsAccountLocked(string username)
        {
            try
            {
                var user = _userRepository.GetUserByUsername(username);
                return user != null && !user.IsActive;
            }
            catch (Exception ex)
            {
                throw new Exception($"Account lock check error: {ex.Message}");
            }
        }

        public bool UnlockAccount(string username)
        {
            try
            {
                var user = _userRepository.GetUserByUsername(username);
                if (user != null)
                {
                    user.IsActive = true;
                    user.UpdatedAt = DateTime.Now;
                    _userRepository.UpdateUser(user);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unlock account error: {ex.Message}");
            }
        }

        public bool ValidateUserSession(int userId)
        {
            try
            {
                // Check if user exists and is active
                var user = _userRepository.GetUserById(userId);
                if (user is null || !user.IsActive)
                    return false;

                // Check if there's a current session and it matches the userId
                if (SessionManager.IsLoggedIn())
                {
                    return SessionManager.GetUserId() == userId;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Validate user session error: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "eShiftSalt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}