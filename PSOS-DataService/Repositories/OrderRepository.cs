using Microsoft.Data.SqlClient;
using PSOS_ApplicationService.DTOs.Orders;
using PSOS_ApplicationService.Exceptions;
using PSOS_ApplicationService.Interfaces.Repositories;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PSOS_DataService.Repositories
{
    public class OrderRepository(SqlConnectionFactory factory) : IOrderRepository
    {

        public async Task<PlaceOrderResult> PlaceOrderAsync(
            int userId, IReadOnlyList<OrderLineRequest> items, CancellationToken ct = default)
        {
            var table = new DataTable();
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Quantity", typeof(int));
            foreach (var item in items)
                table.Rows.Add(item.ProductId, item.Quantity);

            await using var conn = factory.Create();
            await conn.OpenAsync(ct);

            await using var cmd = SqlConnectionFactory.CreateProcedureCommand(conn, "dbo.PlaceOrder");
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            cmd.Parameters.Add(new SqlParameter("@Items", SqlDbType.Structured)
            {
                TypeName = "dbo.OrderItemType",
                Value = table
            });

            try
            {
                await using var reader = await cmd.ExecuteReaderAsync(ct);

                if (reader.FieldCount > 0 && reader.GetName(0) == "OrderId")
                {
                    if (!await reader.ReadAsync(ct))
                        throw new InvalidOperationException("PlaceOrder returned no result.");

                    return new PlaceOrderResult
                    {
                        Success = true,
                        OrderId = reader.GetInt32(0),
                        TotalAmount = reader.GetDecimal(1)
                    };
                }

                var shortages = new List<StockShortage>();
                while (await reader.ReadAsync(ct))
                {
                    shortages.Add(new StockShortage(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3)));
                }
                return new PlaceOrderResult { Success = false, Shortages = shortages };
            }
            catch (SqlException ex)
            {
                throw new ValidationException("One or more products do not exist.");
            }
        }

        public async Task<IReadOnlyList<Order>> GetByUserAsync(int userId, CancellationToken ct = default)
            => await GetOrdersAsync(userId, ct);

        public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default)
            => await GetOrdersAsync(null, ct);

        private async Task<IReadOnlyList<Order>> GetOrdersAsync(int? userId, CancellationToken ct)
        {
            await using var conn = factory.Create();
            await conn.OpenAsync(ct);

            await using var cmd = SqlConnectionFactory.CreateProcedureCommand(conn, "dbo.GetOrders");
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId.HasValue ? userId.Value : DBNull.Value;

            await using var reader = await cmd.ExecuteReaderAsync(ct);

            int orderId = reader.GetOrdinal("OrderId");
            int userIdCol = reader.GetOrdinal("UserId");
            int username = reader.GetOrdinal("Username");
            int orderDate = reader.GetOrdinal("OrderDate");
            int total = reader.GetOrdinal("TotalAmount");
            int status = reader.GetOrdinal("Status");
            int productId = reader.GetOrdinal("ProductId");
            int productName = reader.GetOrdinal("ProductName");
            int quantity = reader.GetOrdinal("Quantity");
            int unitPrice = reader.GetOrdinal("UnitPrice");

            var orders = new List<Order>();
            Order? current = null;

            while (await reader.ReadAsync(ct))
            {
                int id = reader.GetInt32(orderId);
                if (current is null || current.OrderId != id)
                {
                    current = new Order
                    {
                        OrderId = id,
                        UserId = reader.GetInt32(userIdCol),
                        Username = reader.GetString(username),
                        OrderDate = reader.GetDateTime(orderDate),
                        TotalAmount = reader.GetDecimal(total),
                        Status = reader.GetString(status)
                    };
                    orders.Add(current);
                }

                current.Items.Add(new OrderItem
                {
                    ProductId = reader.GetInt32(productId),
                    ProductName = reader.GetString(productName),
                    Quantity = reader.GetInt32(quantity),
                    UnitPrice = reader.GetDecimal(unitPrice)
                });
            }
            return orders;
        }
    }
}
