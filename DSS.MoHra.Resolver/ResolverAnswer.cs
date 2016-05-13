using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public class ResolverAnswer
    {
        public ResolverFact fact { get; set; }
        public bool isFind { get; set; }

        public ResolverAnswer(ResolverFact _fact)
        {
            fact = _fact;
        }
    }
}
