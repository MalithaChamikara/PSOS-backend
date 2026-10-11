using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.DTOs.Auth
{
    public record LoginResponse
    (
        string Token,
        string Username,
        string Role,
        DateTime ExpiresAt
    );
    
}
