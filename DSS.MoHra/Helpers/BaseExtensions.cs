using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSS.MoHra
{
    public static class BaseExtensions
    {
        public static void Import<T>(this T a, T b, string properties)
        {
            var propNames = properties.Split(',');
            var type = a.GetType();
            foreach(var propName in propNames)
            {
                var prop = type.GetProperty(propName);
                if (prop == null)
                    continue;
                prop.SetValue(a, prop.GetValue(b));
            }
        }
    }
}