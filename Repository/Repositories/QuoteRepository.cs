using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class QuoteRepository
    {
        public List<Quote> GetAllQuotes()
        {
            var quotes = new List<Quote>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = @"SELECT q.*, c.first_name, c.last_name FROM quotes q 
                               LEFT JOIN customers c ON q.customer_id = c.customer_id";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            quotes.Add(new Quote
                            {
                                QuoteId = Convert.ToInt32(reader["quote_id"]),
                                CustomerId = Convert.ToInt32(reader["customer_id"]),
                                PickupAddress = reader["pickup_address"].ToString(),
                                DestinationAddress = reader["destination_address"].ToString(),
                                QuoteAmount = Convert.ToDecimal(reader["quote_amount"]),
                                Status = reader["status"].ToString()
                            });
                        }
                    }
                }
            }
            return quotes;
        }

        public int AddQuote(Quote quote)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                string query = @"INSERT INTO quotes 
                    (customer_id, pickup_address, destination_address, estimated_weight, estimated_volume, special_requirements, quote_amount, valid_until, status, created_at, pickup_city, pickup_postal_code, destination_city, destination_postal_code) 
                    VALUES (@customerId, @pickupAddress, @destinationAddress, @estimatedWeight, @estimatedVolume, @specialRequirements, @quoteAmount, @validUntil, @status, @createdAt, @pickupCity, @pickupPostalCode, @destinationCity, @destinationPostalCode);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customerId", quote.CustomerId);
                    command.Parameters.AddWithValue("@pickupAddress", quote.PickupAddress);
                    command.Parameters.AddWithValue("@destinationAddress", quote.DestinationAddress);
                    command.Parameters.AddWithValue("@estimatedWeight", quote.EstimatedWeight);
                    command.Parameters.AddWithValue("@estimatedVolume", quote.EstimatedVolume);
                    command.Parameters.AddWithValue("@specialRequirements", quote.SpecialRequirements);
                    command.Parameters.AddWithValue("@quoteAmount", quote.QuoteAmount);
                    command.Parameters.AddWithValue("@validUntil", quote.ValidUntil);
                    command.Parameters.AddWithValue("@status", quote.Status);
                    command.Parameters.AddWithValue("@createdAt", quote.CreatedAt);
                    command.Parameters.AddWithValue("@pickupCity", quote.PickupCity);
                    command.Parameters.AddWithValue("@pickupPostalCode", quote.PickupPostalCode);
                    command.Parameters.AddWithValue("@destinationCity", quote.DestinationCity);
                    command.Parameters.AddWithValue("@destinationPostalCode", quote.DestinationPostalCode);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }
}