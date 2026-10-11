using System;
using System.Collections.Generic;
using System.Text;

namespace PSOS_ApplicationService.Exceptions
{
    public class ValidationException(string message) : Exception(message);
}
