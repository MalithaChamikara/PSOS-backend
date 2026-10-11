using PSOS_ApplicationService.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_DataService.Utills
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public bool Verify(string password, string hash)
        {
            try { return BCrypt.Net.BCrypt.Verify(password, hash); }
            catch (Exception) { return false; }   
        }
    }
}
