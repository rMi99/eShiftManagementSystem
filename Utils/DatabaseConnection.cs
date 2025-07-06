using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Threading.Tasks;

namespace eShiftManagementSystem.Utils
{
    public static class DatabaseConnection
    {
        private static string? _connectionString;
        private static IConfiguration? _configuration;

        private static IConfiguration Configuration
        {
            get
            {
                if (_configuration is null)
                {
                    // Try to get from Program first
                    if (Program.Configuration is not null)
                    {
                        _configuration = Program.Configuration;
                    }
                    else
                    {
                        // Fallback: create our own configuration
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        _configuration = builder.Build();
                    }
                }
                return _configuration;
            }
        }

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = Configuration.GetConnectionString("DefaultConnection") 
                        ?? "Server=localhost;Database=eshift_db;Uid=root;Pwd=;SslMode=None;AllowUserVariables=true;";
                }
                return _connectionString;
            }
        }

        public static string BackupConnectionString
        {
            get
            {
                return Configuration.GetConnectionString("BackupConnection") 
                    ?? "Server=localhost;Database=eshift_db_backup;Uid=root;Pwd=;SslMode=None;";
            }
        }

        public static MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(ConnectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception($"Database connection failed: {ex.Message}");
            }
        }

        public static MySqlConnection GetBackupConnection()
        {
            try
            {
                var connection = new MySqlConnection(BackupConnectionString);
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception($"Backup database connection failed: {ex.Message}");
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Helper method to get application settings
        public static T GetAppSetting<T>(string key, T defaultValue = default!)
        {
            try
            {
                var value = Configuration[key];
                if (value == null) return defaultValue;
                
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        // Example usage for your application settings
        public static string GetApplicationName() => GetAppSetting("Application:Name", "e-Shift Management System");
        public static string GetApplicationVersion() => GetAppSetting("Application:Version", "1.0.0");
        public static int GetSessionTimeoutMinutes() => GetAppSetting("Security:SessionTimeoutMinutes", 30);
        public static int GetMaxLoginAttempts() => GetAppSetting("Security:MaxLoginAttempts", 5);
    }
}