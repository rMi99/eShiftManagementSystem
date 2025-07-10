using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace eShift.DataAccess
{
    public class DbConnectionFactory
    {
        private static readonly string ConnectionStringName = "eShiftDb";

        public static IDbConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback or default if not found, though ideally it should always be present
                // For development, you might have a hardcoded default or a specific error message
                // In a real app, this might throw a configuration exception
                // For now, let's assume it's found or throw a simpler exception.
                throw new ConfigurationErrorsException($"Connection string '{ConnectionStringName}' not found in App.config.");
            }

            // Ensure the MySql.Data.MySqlClient provider is registered if not using Entity Framework
            // This might be needed if you are using ADO.NET directly without EF's auto-registration
            // DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlConnectorFactory.Instance);


            IDbConnection connection = new MySqlConnection(connectionString);
            return connection;
        }
    }
}
