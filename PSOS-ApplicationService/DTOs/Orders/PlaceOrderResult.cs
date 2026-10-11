using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.DTOs.Orders
{
    public class PlaceOrderResult
    {
        public bool Success { get; init; }
        public int OrderId { get; init; }
        public decimal TotalAmount { get; init; }
        public List<StockShortage> Shortages { get; init; } = new();
    }
}
