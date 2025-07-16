using System;
using System.Text.RegularExpressions;

namespace eShiftManagementSystem.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return Regex.IsMatch(phone, @"^\d{10,15}$");
        }

        public static (bool IsStrong, string ErrorMessage) IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return (false, "Password must be at least 8 characters long.");
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return (false, "Password must contain an uppercase letter.");
            if (!Regex.IsMatch(password, @"[a-z]"))
                return (false, "Password must contain a lowercase letter.");
            if (!Regex.IsMatch(password, @"\d"))
                return (false, "Password must contain a number.");
            if (!Regex.IsMatch(password, @"[\W_]")) // Special character
                return (false, "Password must contain a special character.");
            return (true, "Strong password");
        }
    }
}
