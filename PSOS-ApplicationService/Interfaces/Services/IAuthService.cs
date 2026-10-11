using PSOS_ApplicationService.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
