using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSS.MoHra
{
    public class MetaException : ArgumentException
    {
        public MetaException(string message) : base(message) { }

        public MetaException(string message, string paramName) : base(message, paramName) { }
    }
}