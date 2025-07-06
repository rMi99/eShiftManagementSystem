namespace eShiftManagementSystem.Utils
{
    /// <summary>
    /// Application constants and commonly used strings
    /// </summary>
    public static class Constants
    {
        // User Roles
        public static class Roles
        {
            public const string Admin = "admin";
            public const string Customer = "customer";
            public const string Driver = "driver";
            public const string Staff = "staff";
            public const string Manager = "manager";
            public const string OperationsManager = "operations_manager";
        }

        // Job Statuses
        public static class JobStatus
        {
            public const string Pending = "pending";
            public const string Approved = "approved";
            public const string Assigned = "assigned";
            public const string InProgress = "in_progress";
            public const string PickedUp = "picked_up";
            public const string InTransit = "in_transit";
            public const string Delivered = "delivered";
            public const string Completed = "completed";
            public const string Cancelled = "cancelled";
            public const string Delayed = "delayed";
        }

        // Quote Statuses
        public static class QuoteStatus
        {
            public const string Pending = "pending";
            public const string Accepted = "accepted";
            public const string Declined = "declined";
            public const string Expired = "expired";
        }

        // Driver Statuses
        public static class DriverStatus
        {
            public const string Available = "available";
            public const string OnJob = "on_job";
            public const string Unavailable = "unavailable";
            public const string OffDuty = "off_duty";
        }

        // Job Priorities
        public static class JobPriority
        {
            public const string Low = "low";
            public const string Normal = "normal";
            public const string High = "high";
            public const string Urgent = "urgent";
        }

        // Countries
        public static class Countries
        {
            public const string UnitedKingdom = "United Kingdom";
            public const string UnitedStates = "United States";
            public const string Canada = "Canada";
            public const string Australia = "Australia";
            public const string Ireland = "Ireland";
        }

        // Date Formats
        public static class DateFormats
        {
            public const string Display = "dd/MM/yyyy";
            public const string DisplayWithTime = "dd/MM/yyyy HH:mm";
            public const string Logging = "yyyy-MM-dd HH:mm:ss";
            public const string FileName = "yyyyMMdd";
            public const string ISO8601 = "yyyy-MM-ddTHH:mm:ss";
        }

        // Validation Constants
        public static class Validation
        {
            public const int MinPasswordLength = 8;
            public const int MaxPasswordLength = 50;
            public const int MinUsernameLength = 3;
            public const int MaxUsernameLength = 30;
            public const int MaxDescriptionLength = 1000;
            public const int MaxNotesLength = 500;
            public const int MaxAddressLength = 200;
            public const int MaxNameLength = 50;
            public const int MaxPhoneLength = 20;
        }

        // Application Messages
        public static class Messages
        {
            public const string Success = "Operation completed successfully.";
            public const string Error = "An error occurred. Please try again.";
            public const string ValidationError = "Please check your input and try again.";
            public const string NotFound = "The requested item was not found.";
            public const string Unauthorized = "You are not authorized to perform this action.";
            public const string DatabaseError = "A database error occurred. Please contact support.";
            public const string NetworkError = "A network error occurred. Please check your connection.";
        }

        // File Extensions
        public static class FileExtensions
        {
            public const string Pdf = ".pdf";
            public const string Excel = ".xlsx";
            public const string Csv = ".csv";
            public const string Log = ".log";
        }

        // Application Settings Keys
        public static class AppSettings
        {
            public const string DatabaseConnection = "DatabaseConnection";
            public const string LogLevel = "LogLevel";
            public const string ApplicationName = "ApplicationName";
            public const string Version = "Version";
        }
    }
}