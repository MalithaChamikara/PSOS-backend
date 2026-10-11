using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Exceptions
{
    public class UnauthorizedLoginException() : Exception("Invalid username or password.");
}
