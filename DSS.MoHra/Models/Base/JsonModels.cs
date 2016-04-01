using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSS.MoHra
{
    public class JsonResponse
    {
        // main properties
        public bool Success { set; get; }
        public string Message { set; get; }
        public object Data { set; get; }

        // validation properties
        public object ModelErrors { set; get; }
        
        // navigate peoperties
        public bool NeedReload { set; get; }
        public bool NeedRedirect { set; get; } 
        public string RedirectUrl { set; get; }
    }
}