using Microsoft.IdentityModel.Tokens;
using PSOS_ApplicationService.Interfaces.Services;
using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PSOS_DataService.Utills
{
    public class JwtTokenService(IConfiguration config) : ITokenService
    {
        public (string Token, DateTime ExpiresAt) Create(User user)
        {
            var key = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
            var minutes = int.TryParse(config["Jwt:ExpiryMinutes"], out var m) ? m : 60;
            var expires = DateTime.UtcNow.AddMinutes(minutes);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }
}
