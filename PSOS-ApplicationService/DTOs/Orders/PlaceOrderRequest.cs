using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.DTOs.Orders
{
    public record PlaceOrderRequest
    (
        List<OrderLineRequest> Items
    );
}
