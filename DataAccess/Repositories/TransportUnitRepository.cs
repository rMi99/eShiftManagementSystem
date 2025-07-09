using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class TransportUnitRepository
    {
        public TransportUnit? GetTransportUnitById(int transportUnitId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT tu.*, v.registration_number, v.make, v.model, 
                                   s_d.first_name as driver_fname, s_d.last_name as driver_lname,
                                   s_a.first_name as assistant_fname, s_a.last_name as assistant_lname,
                                   c.container_number, c.type as container_type
                                   FROM transport_units tu 
                                   LEFT JOIN vehicles v ON tu.vehicle_id = v.vehicle_id
                                   LEFT JOIN drivers d ON tu.driver_id = d.driver_id
                                   LEFT JOIN staff s_d ON d.staff_id = s_d.staff_id
                                   LEFT JOIN assistants a ON tu.assistant_id = a.assistant_id
                                   LEFT JOIN staff s_a ON a.staff_id = s_a.staff_id
                                   LEFT JOIN containers c ON tu.container_id = c.container_id
                                   WHERE tu.transport_unit_id = @transportUnitId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@transportUnitId", transportUnitId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TransportUnit
                                {
                                    TransportUnitId = Convert.ToInt32(reader["transport_unit_id"]),
                                    UnitName = reader["unit_name"].ToString() ?? string.Empty,
                                    VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                                    DriverId = reader["driver_id"] != DBNull.Value ? Convert.ToInt32(reader["driver_id"]) : null,
                                    AssistantId = reader["assistant_id"] != DBNull.Value ? Convert.ToInt32(reader["assistant_id"]) : null,
                                    ContainerId = reader["container_id"] != DBNull.Value ? Convert.ToInt32(reader["container_id"]) : null,
                                    IsActive = Convert.ToBoolean(reader["is_active"]),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                    Vehicle = new Vehicle
                                    {
                                        VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                                        RegistrationNumber = reader["registration_number"].ToString() ?? string.Empty,
                                        Make = reader["make"].ToString() ?? string.Empty,
                                        Model = reader["model"].ToString() ?? string.Empty
                                    },
                                    Driver = reader["driver_id"] != DBNull.Value ? new Driver
                                    {
                                        DriverId = Convert.ToInt32(reader["driver_id"]),
                                        Staff = new Staff
                                        {
                                            FirstName = reader["driver_fname"].ToString() ?? string.Empty,
                                            LastName = reader["driver_lname"].ToString() ?? string.Empty
                                        }
                                    } : null,
                                    Assistant = reader["assistant_id"] != DBNull.Value ? new Assistant
                                    {
                                        AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                        Staff = new Staff
                                        {
                                            FirstName = reader["assistant_fname"].ToString() ?? string.Empty,
                                            LastName = reader["assistant_lname"].ToString() ?? string.Empty
                                        }
                                    } : null,
                                    Container = reader["container_id"] != DBNull.Value ? new Container
                                    {
                                        ContainerId = Convert.ToInt32(reader["container_id"]),
                                        ContainerNumber = reader["container_number"].ToString() ?? string.Empty,
                                        Type = reader["container_type"].ToString() ?? string.Empty
                                    } : null
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving transport unit: {ex.Message}");
                }
            }
            return null;
        }

        public List<TransportUnit> GetAllTransportUnits()
        {
            var transportUnits = new List<TransportUnit>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT tu.*, v.registration_number, v.make, v.model, 
                                   s_d.first_name as driver_fname, s_d.last_name as driver_lname,
                                   s_a.first_name as assistant_fname, s_a.last_name as assistant_lname,
                                   c.container_number, c.type as container_type
                                   FROM transport_units tu 
                                   LEFT JOIN vehicles v ON tu.vehicle_id = v.vehicle_id
                                   LEFT JOIN drivers d ON tu.driver_id = d.driver_id
                                   LEFT JOIN staff s_d ON d.staff_id = s_d.staff_id
                                   LEFT JOIN assistants a ON tu.assistant_id = a.assistant_id
                                   LEFT JOIN staff s_a ON a.staff_id = s_a.staff_id
                                   LEFT JOIN containers c ON tu.container_id = c.container_id
                                   ORDER BY tu.unit_name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transportUnits.Add(new TransportUnit
                            {
                                TransportUnitId = Convert.ToInt32(reader["transport_unit_id"]),
                                UnitName = reader["unit_name"].ToString() ?? string.Empty,
                                VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                                DriverId = reader["driver_id"] != DBNull.Value ? Convert.ToInt32(reader["driver_id"]) : null,
                                AssistantId = reader["assistant_id"] != DBNull.Value ? Convert.ToInt32(reader["assistant_id"]) : null,
                                ContainerId = reader["container_id"] != DBNull.Value ? Convert.ToInt32(reader["container_id"]) : null,
                                IsActive = Convert.ToBoolean(reader["is_active"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                Vehicle = new Vehicle
                                {
                                    VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                                    RegistrationNumber = reader["registration_number"].ToString() ?? string.Empty,
                                    Make = reader["make"].ToString() ?? string.Empty,
                                    Model = reader["model"].ToString() ?? string.Empty
                                },
                                Driver = reader["driver_id"] != DBNull.Value ? new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    Staff = new Staff
                                    {
                                        FirstName = reader["driver_fname"].ToString() ?? string.Empty,
                                        LastName = reader["driver_lname"].ToString() ?? string.Empty
                                    }
                                } : null,
                                Assistant = reader["assistant_id"] != DBNull.Value ? new Assistant
                                {
                                    AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                    Staff = new Staff
                                    {
                                        FirstName = reader["assistant_fname"].ToString() ?? string.Empty,
                                        LastName = reader["assistant_lname"].ToString() ?? string.Empty
                                    }
                                } : null,
                                Container = reader["container_id"] != DBNull.Value ? new Container
                                {
                                    ContainerId = Convert.ToInt32(reader["container_id"]),
                                    ContainerNumber = reader["container_number"].ToString() ?? string.Empty,
                                    Type = reader["container_type"].ToString() ?? string.Empty
                                } : null
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving transport units: {ex.Message}");
                }
            }
            return transportUnits;
        }

        public List<TransportUnit> GetActiveTransportUnits()
        {
            var transportUnits = new List<TransportUnit>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT tu.*, v.registration_number, v.make, v.model, 
                                   s_d.first_name as driver_fname, s_d.last_name as driver_lname,
                                   s_a.first_name as assistant_fname, s_a.last_name as assistant_lname,
                                   c.container_number, c.type as container_type
                                   FROM transport_units tu 
                                   LEFT JOIN vehicles v ON tu.vehicle_id = v.vehicle_id
                                   LEFT JOIN drivers d ON tu.driver_id = d.driver_id
                                   LEFT JOIN staff s_d ON d.staff_id = s_d.staff_id
                                   LEFT JOIN assistants a ON tu.assistant_id = a.assistant_id
                                   LEFT JOIN staff s_a ON a.staff_id = s_a.staff_id
                                   LEFT JOIN containers c ON tu.container_id = c.container_id
                                   WHERE tu.is_active = TRUE
                                   ORDER BY tu.unit_name";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transportUnits.Add(new TransportUnit
                            {
                                TransportUnitId = Convert.ToInt32(reader["transport_unit_id"]),
                                UnitName = reader["unit_name"].ToString() ?? string.Empty,
                                VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                                DriverId = reader["driver_id"] != DBNull.Value ? Convert.ToInt32(reader["driver_id"]) : null,
                                AssistantId = reader["assistant_id"] != DBNull.Value ? Convert.ToInt32(reader["assistant_id"]) : null,
                                ContainerId = reader["container_id"] != DBNull.Value ? Convert.ToInt32(reader["container_id"]) : null,
                                IsActive = Convert.ToBoolean(reader["is_active"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                                Vehicle = new Vehicle
                                {
                                    VehicleId = Convert.ToInt32(reader["vehicle_id"]),
                                    RegistrationNumber = reader["registration_number"].ToString() ?? string.Empty,
                                    Make = reader["make"].ToString() ?? string.Empty,
                                    Model = reader["model"].ToString() ?? string.Empty
                                },
                                Driver = reader["driver_id"] != DBNull.Value ? new Driver
                                {
                                    DriverId = Convert.ToInt32(reader["driver_id"]),
                                    Staff = new Staff
                                    {
                                        FirstName = reader["driver_fname"].ToString() ?? string.Empty,
                                        LastName = reader["driver_lname"].ToString() ?? string.Empty
                                    }
                                } : null,
                                Assistant = reader["assistant_id"] != DBNull.Value ? new Assistant
                                {
                                    AssistantId = Convert.ToInt32(reader["assistant_id"]),
                                    Staff = new Staff
                                    {
                                        FirstName = reader["assistant_fname"].ToString() ?? string.Empty,
                                        LastName = reader["assistant_lname"].ToString() ?? string.Empty
                                    }
                                } : null,
                                Container = reader["container_id"] != DBNull.Value ? new Container
                                {
                                    ContainerId = Convert.ToInt32(reader["container_id"]),
                                    ContainerNumber = reader["container_number"].ToString() ?? string.Empty,
                                    Type = reader["container_type"].ToString() ?? string.Empty
                                } : null
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving active transport units: {ex.Message}");
                }
            }
            return transportUnits;
        }

        public int AddTransportUnit(TransportUnit transportUnit)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO transport_units (unit_name, vehicle_id, driver_id, assistant_id, 
                                   container_id, is_active) 
                                   VALUES (@unitName, @vehicleId, @driverId, @assistantId, @containerId, @isActive);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@unitName", transportUnit.UnitName);
                        command.Parameters.AddWithValue("@vehicleId", transportUnit.VehicleId);
                        command.Parameters.AddWithValue("@driverId", transportUnit.DriverId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@assistantId", transportUnit.AssistantId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@containerId", transportUnit.ContainerId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isActive", transportUnit.IsActive);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding transport unit: {ex.Message}");
                }
            }
        }

        public void UpdateTransportUnit(TransportUnit transportUnit)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE transport_units SET unit_name = @unitName, vehicle_id = @vehicleId, 
                                   driver_id = @driverId, assistant_id = @assistantId, container_id = @containerId, 
                                   is_active = @isActive, updated_at = CURRENT_TIMESTAMP 
                                   WHERE transport_unit_id = @transportUnitId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@unitName", transportUnit.UnitName);
                        command.Parameters.AddWithValue("@vehicleId", transportUnit.VehicleId);
                        command.Parameters.AddWithValue("@driverId", transportUnit.DriverId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@assistantId", transportUnit.AssistantId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@containerId", transportUnit.ContainerId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isActive", transportUnit.IsActive);
                        command.Parameters.AddWithValue("@transportUnitId", transportUnit.TransportUnitId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating transport unit: {ex.Message}");
                }
            }
        }

        public void DeleteTransportUnit(int transportUnitId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM transport_units WHERE transport_unit_id = @transportUnitId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@transportUnitId", transportUnitId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting transport unit: {ex.Message}");
                }
            }
        }

        public void UpdateTransportUnitStatus(int transportUnitId, bool isActive)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE transport_units SET is_active = @isActive, updated_at = CURRENT_TIMESTAMP 
                                   WHERE transport_unit_id = @transportUnitId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@isActive", isActive);
                        command.Parameters.AddWithValue("@transportUnitId", transportUnitId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating transport unit status: {ex.Message}");
                }
            }
        }

        public bool UnitNameExists(string unitName, int? excludeTransportUnitId = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM transport_units WHERE unit_name = @unitName";
                    if (excludeTransportUnitId.HasValue)
                        query += " AND transport_unit_id != @excludeTransportUnitId";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@unitName", unitName);
                        if (excludeTransportUnitId.HasValue)
                            command.Parameters.AddWithValue("@excludeTransportUnitId", excludeTransportUnitId);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking unit name existence: {ex.Message}");
                }
            }
        }
    }
}