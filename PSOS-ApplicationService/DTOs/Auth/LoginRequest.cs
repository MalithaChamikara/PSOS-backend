using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.DTOs.Auth
{
    public record LoginRequest
    (
        string Username,
        string Password
    );
    
}
