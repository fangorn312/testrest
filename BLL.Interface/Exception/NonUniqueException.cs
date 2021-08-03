using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface.Exception
{
    public class NonUniqueException : SystemException
    {
        public NonUniqueException(): base() { }

        public NonUniqueException(string message) : base(message) { }
    }
}
