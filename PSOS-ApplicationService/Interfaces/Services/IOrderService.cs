using PSOS_ApplicationService.DTOs.Orders;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Interfaces.Services
{
    public interface IOrderService
    {
        Task<PlaceOrderResult> PlaceAsync(int userId, PlaceOrderRequest request, CancellationToken ct = default);
        Task<IReadOnlyList<Order>> GetMineAsync(int userId, CancellationToken ct = default);
        Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default);
    }
}
