using eShiftManagementSystem.Models;
using eShiftManagementSystem.DataAccess.Repositories;
using System;

namespace eShiftManagementSystem.Utils
{
    public static class SessionManager
    {
        private static User? _currentUser;
        private static Customer? _currentCustomer;
        private static Driver? _currentDriver;

        public static User? CurrentUser => _currentUser;
        public static Customer? CurrentCustomer => _currentCustomer;
        public static Driver? CurrentDriver => _currentDriver;
        public static bool IsLoggedIn => _currentUser is not null;

        public static void SetCurrentUser(User user)
        {
            _currentUser = user;
            LoadCurrentCustomer();
            LoadCurrentDriver();
        }

        private static void LoadCurrentCustomer()
        {
            if (_currentUser is not null && IsCustomer())
            {
                try
                {
                    var customerRepository = new CustomerRepository();
                    _currentCustomer = customerRepository.GetCustomerByUserId(_currentUser.UserId);
                }
                catch
                {
                    _currentCustomer = null;
                }
            }
            else
            {
                _currentCustomer = null;
            }
        }

        private static void LoadCurrentDriver()
        {
            if (_currentUser is not null && IsDriver())
            {
                try
                {
                    var driverRepository = new DriverRepository();
                    _currentDriver = driverRepository.GetDriverByUserId(_currentUser.UserId);
                }
                catch
                {
                    _currentDriver = null;
                }
            }
            else
            {
                _currentDriver = null;
            }
        }

        public static void Logout()
        {
            _currentUser = null;
            _currentCustomer = null;
            _currentDriver = null;
        }

        public static bool IsLoggedIn()
        {
            return _currentUser is not null;
        }

        public static bool IsAdmin()
        {
            return _currentUser is not null && string.Equals(_currentUser.Role, "admin", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCustomer()
        {
            return _currentUser is not null && string.Equals(_currentUser.Role, "customer", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsOperationsManager()
        {
            return _currentUser is not null && 
                   (string.Equals(_currentUser.Role, "operations_manager", StringComparison.OrdinalIgnoreCase) || 
                    string.Equals(_currentUser.Role, "manager", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(_currentUser.Role, "admin", StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsDriver()
        {
            return _currentUser is not null && string.Equals(_currentUser.Role, "driver", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsStaff()
        {
            return _currentUser is not null && 
                   (string.Equals(_currentUser.Role, "staff", StringComparison.OrdinalIgnoreCase) || 
                    string.Equals(_currentUser.Role, "admin", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(_currentUser.Role, "manager", StringComparison.OrdinalIgnoreCase));
        }

        public static string GetUserDisplayName()
        {
            if (_currentUser is null) return "Guest";
            return !string.IsNullOrEmpty(_currentUser.Username) ? _currentUser.Username : _currentUser.Email;
        }

        public static int GetUserId()
        {
            return _currentUser?.UserId ?? 0;
        }

        public static int GetCustomerId()
        {
            return _currentCustomer?.CustomerId ?? 0;
        }

        public static int GetDriverId()
        {
            return _currentDriver?.DriverId ?? 0;
        }

        public static string GetUserRole()
        {
            return _currentUser?.Role ?? "guest";
        }
    }
}