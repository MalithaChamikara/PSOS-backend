using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSOS_ApplicationService.Interfaces.Services;
using PSOS_BusinessObjects;

namespace PSOS_Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;

        public ProductsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAll(CancellationToken ct)
        {
            return Ok(await _productsService.GetAllAsync(ct));
        }

        [HttpGet("low-stock")]
        [Authorize(Roles = Roles.Administrator)]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetLowStock(CancellationToken ct)
        {
            return Ok(await _productsService.GetLowStockAsync(ct));
        }
    }
}
