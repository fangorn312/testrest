using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface.Exception
{
    public class CustomValidationException : SystemException
    {
        public CustomValidationException(): base() { }

        public CustomValidationException(string message) : base(message) { }
    }
}
