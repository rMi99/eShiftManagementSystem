using System;

namespace eShiftManagementSystem.Utils
{
    /// <summary>
    /// Utility class for date and time operations
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Gets the current date without time component
        /// </summary>
        public static DateTime Today => DateTime.Now.Date;

        /// <summary>
        /// Gets the current date and time
        /// </summary>
        public static DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets the UTC current date and time
        /// </summary>
        public static DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Formats date for display in the application
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <returns>Formatted date string</returns>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Formats date and time for display in the application
        /// </summary>
        /// <param name="dateTime">DateTime to format</param>
        /// <returns>Formatted datetime string</returns>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Formats date and time for logging
        /// </summary>
        /// <param name="dateTime">DateTime to format</param>
        /// <returns>Formatted datetime string for logs</returns>
        public static string FormatForLogging(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Gets the start of the day for the given date
        /// </summary>
        /// <param name="date">Date to get start of day for</param>
        /// <returns>DateTime representing start of day</returns>
        public static DateTime StartOfDay(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Gets the end of the day for the given date
        /// </summary>
        /// <param name="date">Date to get end of day for</param>
        /// <returns>DateTime representing end of day</returns>
        public static DateTime EndOfDay(DateTime date)
        {
            return date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Checks if a date is within the last 30 days
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>True if date is within last 30 days</returns>
        public static bool IsWithinLast30Days(DateTime date)
        {
            return date >= DateTime.Now.AddDays(-30);
        }

        /// <summary>
        /// Checks if a date is within the next 30 days
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>True if date is within next 30 days</returns>
        public static bool IsWithinNext30Days(DateTime date)
        {
            return date <= DateTime.Now.AddDays(30) && date >= DateTime.Now.Date;
        }

        /// <summary>
        /// Gets a human-readable relative time string
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Relative time string (e.g., "2 days ago", "in 3 hours")</returns>
        public static string GetRelativeTime(DateTime date)
        {
            var timeSpan = DateTime.Now - date;
            var totalDays = Math.Abs(timeSpan.TotalDays);
            var totalHours = Math.Abs(timeSpan.TotalHours);
            var totalMinutes = Math.Abs(timeSpan.TotalMinutes);

            if (totalDays >= 1)
            {
                var days = (int)totalDays;
                var suffix = timeSpan.TotalDays > 0 ? "ago" : "from now";
                return $"{days} day{(days != 1 ? "s" : "")} {suffix}";
            }
            else if (totalHours >= 1)
            {
                var hours = (int)totalHours;
                var suffix = timeSpan.TotalHours > 0 ? "ago" : "from now";
                return $"{hours} hour{(hours != 1 ? "s" : "")} {suffix}";
            }
            else if (totalMinutes >= 1)
            {
                var minutes = (int)totalMinutes;
                var suffix = timeSpan.TotalMinutes > 0 ? "ago" : "from now";
                return $"{minutes} minute{(minutes != 1 ? "s" : "")} {suffix}";
            }
            else
            {
                return "just now";
            }
        }
    }
}