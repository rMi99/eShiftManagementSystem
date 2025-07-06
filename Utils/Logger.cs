using System;
using System.IO;

namespace eShiftManagementSystem.Utils
{
    /// <summary>
    /// Simple logging utility for the application
    /// </summary>
    public static class Logger
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string LogFileName = $"eShiftMS_{DateTime.Now:yyyyMMdd}.log";
        private static readonly string LogFilePath = Path.Combine(LogDirectory, LogFileName);
        private static readonly object LockObject = new object();

        static Logger()
        {
            // Ensure log directory exists
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        /// <summary>
        /// Logs an information message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="memberName">Calling member name (auto-populated)</param>
        public static void LogInfo(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            WriteLog("INFO", message, memberName);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">Error message to log</param>
        /// <param name="memberName">Calling member name (auto-populated)</param>
        public static void LogError(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            WriteLog("ERROR", message, memberName);
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="additionalMessage">Additional context message</param>
        /// <param name="memberName">Calling member name (auto-populated)</param>
        public static void LogException(Exception ex, string additionalMessage = "", [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            var message = $"{additionalMessage} Exception: {ex.Message} | StackTrace: {ex.StackTrace}";
            WriteLog("ERROR", message, memberName);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">Warning message to log</param>
        /// <param name="memberName">Calling member name (auto-populated)</param>
        public static void LogWarning(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            WriteLog("WARNING", message, memberName);
        }

        /// <summary>
        /// Logs a debug message (only in debug builds)
        /// </summary>
        /// <param name="message">Debug message to log</param>
        /// <param name="memberName">Calling member name (auto-populated)</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            WriteLog("DEBUG", message, memberName);
        }

        private static void WriteLog(string level, string message, string memberName)
        {
            try
            {
                lock (LockObject)
                {
                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] [{memberName}] {message}";
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                }
            }
            catch
            {
                // Silently fail if logging fails to prevent infinite loops
            }
        }
    }
}