using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.DTOs.Orders
{
    public record OrderLineRequest
    (
        int ProductId, 
        int Quantity
    );
}
