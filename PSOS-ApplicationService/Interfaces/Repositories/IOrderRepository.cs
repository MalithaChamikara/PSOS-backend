using PSOS_ApplicationService.DTOs.Orders;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<PlaceOrderResult> PlaceOrderAsync(int userId, IReadOnlyList<OrderLineRequest> items, CancellationToken ct = default);
        Task<IReadOnlyList<Order>> GetByUserAsync(int userId, CancellationToken ct = default);
        Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default);
    }
}
