using eShiftManagementSystem.DataAccess.Interfaces;
using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class PriceRangeRepository : IPriceRangeRepository
    {
        public List<PriceRange> GetAll()
        {
            var priceRanges = new List<PriceRange>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM price_ranges ORDER BY from_weight";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            priceRanges.Add(new PriceRange
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                FromWeight = Convert.ToDecimal(reader["from_weight"]),
                                ToWeight = Convert.ToDecimal(reader["to_weight"]),
                                Price = Convert.ToDecimal(reader["price"])
                            });
                        }
                    }
                }
            }
            return priceRanges;
        }

        public PriceRange GetById(int id)
        {
            // Implementation to get a single price range by ID
            return null; // Simplified
        }

        public void Add(PriceRange priceRange)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO price_ranges (from_weight, to_weight, price) VALUES (@fromWeight, @toWeight, @price)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fromWeight", priceRange.FromWeight);
                    command.Parameters.AddWithValue("@toWeight", priceRange.ToWeight);
                    command.Parameters.AddWithValue("@price", priceRange.Price);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(PriceRange priceRange)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "UPDATE price_ranges SET from_weight = @fromWeight, to_weight = @toWeight, price = @price WHERE id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fromWeight", priceRange.FromWeight);
                    command.Parameters.AddWithValue("@toWeight", priceRange.ToWeight);
                    command.Parameters.AddWithValue("@price", priceRange.Price);
                    command.Parameters.AddWithValue("@id", priceRange.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM price_ranges WHERE id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}