using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Interfaces.Services
{
    public interface IPasswordHasher
    {
        bool Verify(string password, string hash);
    }
}
