using PSOS_ApplicationService.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Exceptions
{
    public class InsufficientStockException(List<StockShortage> shortages)
    : Exception("Not enough stock: " + string.Join("; ",
        shortages.Select(s => $"{s.ProductName} (requested {s.Requested}, available {s.Available})")))
    {
        public List<StockShortage> Shortages { get; } = shortages;
    }
}
