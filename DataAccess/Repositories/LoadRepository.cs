using eShiftManagementSystem.Models;
using eShiftManagementSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Repositories
{
    public class LoadRepository
    {
        public Load? GetLoadById(int loadId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT l.*, j.job_number, j.customer_id 
                                   FROM loads l 
                                   INNER JOIN jobs j ON l.job_id = j.job_id 
                                   WHERE l.load_id = @loadId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@loadId", loadId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Load
                                {
                                    LoadId = Convert.ToInt32(reader["load_id"]),
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    ProductId = reader["product_id"] != DBNull.Value ? Convert.ToInt32(reader["product_id"]) : 0,
                                    Quantity = reader["quantity"] != DBNull.Value ? Convert.ToInt32(reader["quantity"]) : 0,
                                    Weight = reader["total_weight"] != DBNull.Value ? Convert.ToDecimal(reader["total_weight"]) : null,
                                    Volume = reader["total_volume"] != DBNull.Value ? Convert.ToDecimal(reader["total_volume"]) : null,
                                    Description = reader["description"].ToString() ?? string.Empty,
                                    Job = new Job
                                    {
                                        JobId = Convert.ToInt32(reader["job_id"]),
                                        JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                        CustomerId = Convert.ToInt32(reader["customer_id"])
                                    }
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving load: {ex.Message}");
                }
            }
            return null;
        }

        public List<Load> GetLoadsByJobId(int jobId)
        {
            var loads = new List<Load>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT l.*, j.job_number, j.customer_id 
                                   FROM loads l 
                                   INNER JOIN jobs j ON l.job_id = j.job_id 
                                   WHERE l.job_id = @jobId 
                                   ORDER BY l.load_number";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jobId", jobId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                loads.Add(new Load
                                {
                                    LoadId = Convert.ToInt32(reader["load_id"]),
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    ProductId = reader["product_id"] != DBNull.Value ? Convert.ToInt32(reader["product_id"]) : 0,
                                    Quantity = reader["quantity"] != DBNull.Value ? Convert.ToInt32(reader["quantity"]) : 0,
                                    Weight = reader["total_weight"] != DBNull.Value ? Convert.ToDecimal(reader["total_weight"]) : null,
                                    Volume = reader["total_volume"] != DBNull.Value ? Convert.ToDecimal(reader["total_volume"]) : null,
                                    Description = reader["description"].ToString() ?? string.Empty,
                                    Job = new Job
                                    {
                                        JobId = Convert.ToInt32(reader["job_id"]),
                                        JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                        CustomerId = Convert.ToInt32(reader["customer_id"])
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving loads for job: {ex.Message}");
                }
            }
            return loads;
        }

        public List<Load> GetAllLoads()
        {
            var loads = new List<Load>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT l.*, j.job_number, j.customer_id 
                                   FROM loads l 
                                   INNER JOIN jobs j ON l.job_id = j.job_id 
                                   ORDER BY l.created_at DESC";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            loads.Add(new Load
                            {
                                LoadId = Convert.ToInt32(reader["load_id"]),
                                JobId = Convert.ToInt32(reader["job_id"]),
                                ProductId = reader["product_id"] != DBNull.Value ? Convert.ToInt32(reader["product_id"]) : 0,
                                Quantity = reader["quantity"] != DBNull.Value ? Convert.ToInt32(reader["quantity"]) : 0,
                                Weight = reader["total_weight"] != DBNull.Value ? Convert.ToDecimal(reader["total_weight"]) : null,
                                Volume = reader["total_volume"] != DBNull.Value ? Convert.ToDecimal(reader["total_volume"]) : null,
                                Description = reader["description"].ToString() ?? string.Empty,
                                Job = new Job
                                {
                                    JobId = Convert.ToInt32(reader["job_id"]),
                                    JobNumber = reader["job_number"].ToString() ?? string.Empty,
                                    CustomerId = Convert.ToInt32(reader["customer_id"])
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving loads: {ex.Message}");
                }
            }
            return loads;
        }

        public int AddLoad(Load load)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO loads (job_id, load_number, description, total_weight, total_volume, 
                                   is_fragile, special_handling_required, loading_instructions, status) 
                                   VALUES (@jobId, @loadNumber, @description, @totalWeight, @totalVolume, 
                                   @isFragile, @specialHandling, @loadingInstructions, @status);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Generate load number if not provided
                        var loadNumber = string.IsNullOrEmpty(load.Description) ? 
                            $"LOAD-{DateTime.Now:yyyyMMddHHmmss}" : load.Description;
                        
                        command.Parameters.AddWithValue("@jobId", load.JobId);
                        command.Parameters.AddWithValue("@loadNumber", loadNumber);
                        command.Parameters.AddWithValue("@description", load.Description);
                        command.Parameters.AddWithValue("@totalWeight", load.Weight ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@totalVolume", load.Volume ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@isFragile", false); // Default value
                        command.Parameters.AddWithValue("@specialHandling", false); // Default value
                        command.Parameters.AddWithValue("@loadingInstructions", (object)DBNull.Value);
                        command.Parameters.AddWithValue("@status", "pending");
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding load: {ex.Message}");
                }
            }
        }

        public void UpdateLoad(Load load)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE loads SET description = @description, total_weight = @totalWeight, 
                                   total_volume = @totalVolume, updated_at = CURRENT_TIMESTAMP 
                                   WHERE load_id = @loadId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@description", load.Description);
                        command.Parameters.AddWithValue("@totalWeight", load.Weight ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@totalVolume", load.Volume ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@loadId", load.LoadId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating load: {ex.Message}");
                }
            }
        }

        public void DeleteLoad(int loadId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    // First delete load products
                    string deleteProductsQuery = "DELETE FROM load_products WHERE load_id = @loadId";
                    using (var command = new MySqlCommand(deleteProductsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@loadId", loadId);
                        command.ExecuteNonQuery();
                    }
                    
                    // Then delete the load
                    string deleteLoadQuery = "DELETE FROM loads WHERE load_id = @loadId";
                    using (var command = new MySqlCommand(deleteLoadQuery, connection))
                    {
                        command.Parameters.AddWithValue("@loadId", loadId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting load: {ex.Message}");
                }
            }
        }

        public List<LoadProduct> GetLoadProducts(int loadId)
        {
            var loadProducts = new List<LoadProduct>();
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT lp.*, p.product_name, p.description as product_description, 
                                   pc.category_name 
                                   FROM load_products lp 
                                   INNER JOIN products p ON lp.product_id = p.product_id 
                                   LEFT JOIN product_categories pc ON p.category_id = pc.category_id 
                                   WHERE lp.load_id = @loadId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@loadId", loadId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                loadProducts.Add(new LoadProduct
                                {
                                    LoadProductId = Convert.ToInt32(reader["load_product_id"]),
                                    LoadId = Convert.ToInt32(reader["load_id"]),
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    Quantity = Convert.ToInt32(reader["quantity"]),
                                    UnitWeight = reader["unit_weight"] != DBNull.Value ? Convert.ToDecimal(reader["unit_weight"]) : null,
                                    UnitVolume = reader["unit_volume"] != DBNull.Value ? Convert.ToDecimal(reader["unit_volume"]) : null,
                                    TotalWeight = reader["total_weight"] != DBNull.Value ? Convert.ToDecimal(reader["total_weight"]) : null,
                                    TotalVolume = reader["total_volume"] != DBNull.Value ? Convert.ToDecimal(reader["total_volume"]) : null,
                                    ConditionNotes = reader["condition_notes"].ToString(),
                                    Product = new Product
                                    {
                                        ProductId = Convert.ToInt32(reader["product_id"]),
                                        ProductName = reader["product_name"].ToString() ?? string.Empty,
                                        Description = reader["product_description"].ToString() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving load products: {ex.Message}");
                }
            }
            return loadProducts;
        }

        public int AddLoadProduct(LoadProduct loadProduct)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO load_products (load_id, product_id, quantity, unit_weight, unit_volume, 
                                   total_weight, total_volume, condition_notes) 
                                   VALUES (@loadId, @productId, @quantity, @unitWeight, @unitVolume, 
                                   @totalWeight, @totalVolume, @conditionNotes);
                                   SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@loadId", loadProduct.LoadId);
                        command.Parameters.AddWithValue("@productId", loadProduct.ProductId);
                        command.Parameters.AddWithValue("@quantity", loadProduct.Quantity);
                        command.Parameters.AddWithValue("@unitWeight", loadProduct.UnitWeight ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@unitVolume", loadProduct.UnitVolume ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@totalWeight", loadProduct.TotalWeight ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@totalVolume", loadProduct.TotalVolume ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@conditionNotes", loadProduct.ConditionNotes ?? (object)DBNull.Value);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding load product: {ex.Message}");
                }
            }
        }

        public void UpdateLoadProduct(LoadProduct loadProduct)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE load_products SET quantity = @quantity, unit_weight = @unitWeight, 
                                   unit_volume = @unitVolume, total_weight = @totalWeight, total_volume = @totalVolume, 
                                   condition_notes = @conditionNotes 
                                   WHERE load_product_id = @loadProductId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@quantity", loadProduct.Quantity);
                        command.Parameters.AddWithValue("@unitWeight", loadProduct.UnitWeight ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@unitVolume", loadProduct.UnitVolume ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@totalWeight", loadProduct.TotalWeight ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@totalVolume", loadProduct.TotalVolume ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@conditionNotes", loadProduct.ConditionNotes ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@loadProductId", loadProduct.LoadProductId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating load product: {ex.Message}");
                }
            }
        }

        public void DeleteLoadProduct(int loadProductId)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM load_products WHERE load_product_id = @loadProductId";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@loadProductId", loadProductId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting load product: {ex.Message}");
                }
            }
        }
    }
}