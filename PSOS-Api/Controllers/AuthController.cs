using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSOS_ApplicationService.DTOs.Auth;
using PSOS_ApplicationService.Interfaces.Services;

namespace PSOS_Api.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            return Ok(await _authService.LoginAsync(request, ct));
        }

    }
}
