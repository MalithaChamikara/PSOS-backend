using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.DTOs.Orders
{
    public record StockShortage
    (
        int ProductId, 
        string ProductName, 
        int Available, 
        int Requested
    );
}
