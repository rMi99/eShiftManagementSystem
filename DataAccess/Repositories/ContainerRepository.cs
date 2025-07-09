using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class ContainerRepository
    {
        public Container? GetContainerById(int containerId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM containers WHERE container_id = @containerId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@containerId", containerId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Container
                                {
                                    ContainerId = Convert.ToInt32(reader["container_id"]),
                                    ContainerNumber = reader["container_number"].ToString() ?? string.Empty,
                                    Type = reader["type"].ToString() ?? string.Empty,
                                    MaxWeight = Convert.ToDecimal(reader["max_weight"]),
                                    MaxVolume = Convert.ToDecimal(reader["max_volume"]),
                                    IsAvailable = Convert.ToBoolean(reader["is_available"])
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving container: {ex.Message}");
                }
            }
            return null;
        }

        public List<Container> GetAllContainers()
        {
            var containers = new List<Container>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM containers ORDER BY container_number";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            containers.Add(new Container
                            {
                                ContainerId = Convert.ToInt32(reader["container_id"]),
                                ContainerNumber = reader["container_number"].ToString() ?? string.Empty,
                                Type = reader["type"].ToString() ?? string.Empty,
                                MaxWeight = Convert.ToDecimal(reader["max_weight"]),
                                MaxVolume = Convert.ToDecimal(reader["max_volume"]),
                                IsAvailable = Convert.ToBoolean(reader["is_available"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving containers: {ex.Message}");
                }
            }
            return containers;
        }

        public List<Container> GetAvailableContainers()
        {
            var containers = new List<Container>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM containers WHERE is_available = TRUE ORDER BY container_number";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            containers.Add(new Container
                            {
                                ContainerId = Convert.ToInt32(reader["container_id"]),
                                ContainerNumber = reader["container_number"].ToString() ?? string.Empty,
                                Type = reader["type"].ToString() ?? string.Empty,
                                MaxWeight = Convert.ToDecimal(reader["max_weight"]),
                                MaxVolume = Convert.ToDecimal(reader["max_volume"]),
                                IsAvailable = Convert.ToBoolean(reader["is_available"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving available containers: {ex.Message}");
                }
            }
            return containers;
        }

        public int AddContainer(Container container)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO containers (container_number, type, max_weight, max_volume, is_available) 
                                   VALUES (@containerNumber, @type, @maxWeight, @maxVolume, @isAvailable);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@containerNumber", container.ContainerNumber);
                        command.Parameters.AddWithValue("@type", container.Type);
                        command.Parameters.AddWithValue("@maxWeight", container.MaxWeight);
                        command.Parameters.AddWithValue("@maxVolume", container.MaxVolume);
                        command.Parameters.AddWithValue("@isAvailable", container.IsAvailable);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding container: {ex.Message}");
                }
            }
        }

        public void UpdateContainer(Container container)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE containers SET container_number = @containerNumber, type = @type, 
                                   max_weight = @maxWeight, max_volume = @maxVolume, is_available = @isAvailable 
                                   WHERE container_id = @containerId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@containerNumber", container.ContainerNumber);
                        command.Parameters.AddWithValue("@type", container.Type);
                        command.Parameters.AddWithValue("@maxWeight", container.MaxWeight);
                        command.Parameters.AddWithValue("@maxVolume", container.MaxVolume);
                        command.Parameters.AddWithValue("@isAvailable", container.IsAvailable);
                        command.Parameters.AddWithValue("@containerId", container.ContainerId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating container: {ex.Message}");
                }
            }
        }

        public void DeleteContainer(int containerId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM containers WHERE container_id = @containerId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@containerId", containerId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting container: {ex.Message}");
                }
            }
        }

        public bool ContainerExists(string containerNumber)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM containers WHERE container_number = @containerNumber";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@containerNumber", containerNumber);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error checking container existence: {ex.Message}");
                }
            }
        }
    }
}