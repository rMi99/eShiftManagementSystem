using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class QuoteRepository
    {
        public Quote? GetQuoteById(int quoteId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT q.*, c.first_name, c.last_name FROM quotes q 
                                   LEFT JOIN customers c ON q.customer_id = c.customer_id 
                                   WHERE q.quote_id = @quoteId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@quoteId", quoteId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Quote
                                {
                                    QuoteId = Convert.ToInt32(reader["quote_id"]),
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    QuoteNumber = reader["quote_number"].ToString() ?? string.Empty,
                                    EstimatedCost = Convert.ToDecimal(reader["estimated_cost"]),
                                    Description = reader["description"].ToString() ?? string.Empty,
                                    ValidUntil = Convert.ToDateTime(reader["valid_until"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty
                                    }
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving quote: {ex.Message}");
                }
            }
            return null;
        }

        public List<Quote> GetAllQuotes()
        {
            var quotes = new List<Quote>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT q.*, c.first_name, c.last_name FROM quotes q 
                                   LEFT JOIN customers c ON q.customer_id = c.customer_id 
                                   ORDER BY q.created_at DESC";
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
                                    QuoteNumber = reader["quote_number"].ToString() ?? string.Empty,
                                    EstimatedCost = Convert.ToDecimal(reader["estimated_cost"]),
                                    Description = reader["description"].ToString() ?? string.Empty,
                                    ValidUntil = Convert.ToDateTime(reader["valid_until"]),
                                    Status = reader["status"].ToString() ?? string.Empty,
                                    CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                    Customer = new Customer
                                    {
                                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                                        FirstName = reader["first_name"].ToString() ?? string.Empty,
                                        LastName = reader["last_name"].ToString() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving quotes: {ex.Message}");
                }
            }
            return quotes;
        }

        public int AddQuote(Quote quote)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO quotes (customer_id, quote_number, estimated_cost, 
                                   description, valid_until, status, created_at) 
                                   VALUES (@customerId, @quoteNumber, @estimatedCost, @description, 
                                   @validUntil, @status, @createdAt);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", quote.CustomerId);
                        command.Parameters.AddWithValue("@quoteNumber", quote.QuoteNumber);
                        command.Parameters.AddWithValue("@estimatedCost", quote.EstimatedCost);
                        command.Parameters.AddWithValue("@description", quote.Description);
                        command.Parameters.AddWithValue("@validUntil", quote.ValidUntil);
                        command.Parameters.AddWithValue("@status", quote.Status);
                        command.Parameters.AddWithValue("@createdAt", quote.CreatedAt);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding quote: {ex.Message}");
                }
            }
        }

        public void UpdateQuote(Quote quote)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE quotes SET customer_id = @customerId, quote_number = @quoteNumber, 
                                   estimated_cost = @estimatedCost, description = @description, 
                                   valid_until = @validUntil, status = @status 
                                   WHERE quote_id = @quoteId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", quote.CustomerId);
                        command.Parameters.AddWithValue("@quoteNumber", quote.QuoteNumber);
                        command.Parameters.AddWithValue("@estimatedCost", quote.EstimatedCost);
                        command.Parameters.AddWithValue("@description", quote.Description);
                        command.Parameters.AddWithValue("@validUntil", quote.ValidUntil);
                        command.Parameters.AddWithValue("@status", quote.Status);
                        command.Parameters.AddWithValue("@quoteId", quote.QuoteId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating quote: {ex.Message}");
                }
            }
        }

        public void DeleteQuote(int quoteId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM quotes WHERE quote_id = @quoteId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@quoteId", quoteId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting quote: {ex.Message}");
                }
            }
        }
    }
}