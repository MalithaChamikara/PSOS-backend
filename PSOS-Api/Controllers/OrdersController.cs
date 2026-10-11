using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSOS_ApplicationService.DTOs.Orders;
using PSOS_ApplicationService.Interfaces.Services;
using PSOS_BusinessObjects;
using System.Security.Claims;

namespace PSOS_Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        [ProducesResponseType(typeof(PlaceOrderResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PlaceOrderResult>> Place([FromBody] PlaceOrderRequest request, CancellationToken ct)
        {
            return  Ok(await _orderService.PlaceAsync(CurrentUserId,request, ct));
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IReadOnlyList<Order>>> MyOrders(CancellationToken ct)
        {
            return Ok(await _orderService.GetMineAsync(CurrentUserId, ct));
        }

        [HttpGet]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetAll(CancellationToken ct)
        {
            return Ok(await _orderService.GetAllAsync(ct));
        }


    }
}
