using System;
using System.Text.RegularExpressions;

namespace eShiftManagementSystem.Utils
{
    /// <summary>
    /// Utility class for common validation operations
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates if an email address is in correct format
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if email is valid, false otherwise</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates phone number format
        /// </summary>
        /// <param name="phone">Phone number to validate</param>
        /// <returns>True if phone number is valid, false otherwise</returns>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remove all non-numeric characters for validation
            var numericPhone = Regex.Replace(phone, @"[^\d]", "");
            
            // Check if it's between 10 and 15 digits (international format)
            return numericPhone.Length >= 10 && numericPhone.Length <= 15;
        }

        /// <summary>
        /// Validates UK postal code format
        /// </summary>
        /// <param name="postalCode">Postal code to validate</param>
        /// <returns>True if postal code is valid, false otherwise</returns>
        public static bool IsValidUKPostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return false;

            // UK postal code pattern
            var ukPostalCodePattern = @"^[A-Z]{1,2}[0-9][A-Z0-9]?\s?[0-9][A-Z]{2}$";
            return Regex.IsMatch(postalCode.ToUpperInvariant(), ukPostalCodePattern);
        }

        /// <summary>
        /// Validates password strength
        /// </summary>
        /// <param name="password">Password to validate</param>
        /// <returns>True if password meets minimum requirements, false otherwise</returns>
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Password must be at least 8 characters long
            if (password.Length < 8)
                return false;

            // Must contain at least one uppercase letter, one lowercase letter, and one digit
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

            return hasUpper && hasLower && hasDigit;
        }

        /// <summary>
        /// Validates if a string is not null, empty, or whitespace
        /// </summary>
        /// <param name="value">String to validate</param>
        /// <returns>True if string has value, false otherwise</returns>
        public static bool HasValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Validates if a decimal value is positive
        /// </summary>
        /// <param name="value">Decimal value to validate</param>
        /// <returns>True if value is positive, false otherwise</returns>
        public static bool IsPositive(decimal value)
        {
            return value > 0;
        }

        /// <summary>
        /// Validates if a date is not in the past
        /// </summary>
        /// <param name="date">Date to validate</param>
        /// <returns>True if date is today or in the future, false otherwise</returns>
        public static bool IsNotInPast(DateTime date)
        {
            return date.Date >= DateTime.Now.Date;
        }

        /// <summary>
        /// Validates if a date is within a reasonable future range (within 2 years)
        /// </summary>
        /// <param name="date">Date to validate</param>
        /// <returns>True if date is within reasonable range, false otherwise</returns>
        public static bool IsWithinReasonableFuture(DateTime date)
        {
            return date.Date <= DateTime.Now.Date.AddYears(2);
        }
    }
}