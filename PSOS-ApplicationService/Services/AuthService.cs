using PSOS_ApplicationService.DTOs.Auth;
using PSOS_ApplicationService.Exceptions;
using PSOS_ApplicationService.Interfaces.Repositories;
using PSOS_ApplicationService.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSOS_ApplicationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasher hasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrEmpty(request.Password))
                throw new ValidationException("Username and password are required.");

            var user = await _userRepository.GetByUsernameAsync(request.Username.Trim(), ct);
            if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedLoginException();

            var (token, expires) = _tokenService.Create(user);
            return new LoginResponse(token, user.Username, user.Role, expires);
        }
    }
}
