using PSOS_ApplicationService.DTOs.Orders;
using PSOS_ApplicationService.Exceptions;
using PSOS_ApplicationService.Interfaces.Repositories;
using PSOS_ApplicationService.Interfaces.Services;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _ordersRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _ordersRepository = orderRepository;
        }
        public async Task<PlaceOrderResult> PlaceAsync(
            int userId, PlaceOrderRequest request, CancellationToken ct = default)
        {
            if (request?.Items is null || request.Items.Count == 0)
                throw new ValidationException("An order must contain at least one item.");

            if (request.Items.Any(i => i.ProductId <= 0))
                throw new ValidationException("Invalid product.");

            if (request.Items.Any(i => i.Quantity <= 0))
                throw new ValidationException("Quantity must be greater than zero.");

            var merged = request.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new OrderLineRequest(g.Key, g.Sum(x => x.Quantity)))
                .OrderBy(i => i.ProductId)
                .ToList();

            var result = await _ordersRepository.PlaceOrderAsync(userId, merged, ct);

            if (!result.Success)
                throw new InsufficientStockException(result.Shortages);

            return result;
        }

        public Task<IReadOnlyList<Order>> GetMineAsync(int userId, CancellationToken ct = default)
            => _ordersRepository.GetByUserAsync(userId, ct);

        public Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default)
            => _ordersRepository.GetAllAsync(ct);
    }
}
