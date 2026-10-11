using Microsoft.Data.SqlClient;
using PSOS_ApplicationService.Interfaces.Repositories;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PSOS_DataService.Repositories
{
    public class ProductRepository(SqlConnectionFactory factory) : IProductRepository
    {
        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        {
            await using var conn = factory.Create();
            await conn.OpenAsync(ct);

            await using var cmd = SqlConnectionFactory.CreateProcedureCommand(conn, "dbo.GetProducts");
            return await ReadProductsAsync(cmd, ct);
        }

        public async Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, CancellationToken ct = default)
        {
            await using var conn = factory.Create();
            await conn.OpenAsync(ct);

            await using var cmd = SqlConnectionFactory.CreateProcedureCommand(conn, "dbo.GetLowStockProducts");
            cmd.Parameters.Add("@Threshold", SqlDbType.Int).Value = threshold;
            return await ReadProductsAsync(cmd, ct);
        }

        private static async Task<IReadOnlyList<Product>> ReadProductsAsync(SqlCommand cmd, CancellationToken ct)
        {
            var products = new List<Product>();
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            int idCol = reader.GetOrdinal("ProductId");
            int nameCol = reader.GetOrdinal("Name");
            int priceCol = reader.GetOrdinal("Price");
            int stockCol = reader.GetOrdinal("StockQuantity");

            while (await reader.ReadAsync(ct))
            {
                products.Add(new Product
                {
                    ProductId = reader.GetInt32(idCol),
                    Name = reader.GetString(nameCol),
                    Price = reader.GetDecimal(priceCol),
                    StockQuantity = reader.GetInt32(stockCol)
                });
            }
            return products;
        }
    }
}
