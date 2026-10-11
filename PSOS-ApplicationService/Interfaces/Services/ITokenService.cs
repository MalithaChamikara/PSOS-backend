using PSOS_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Interfaces.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) Create(User user);
    }
}
