using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public interface IResolver
    {
        void AddRule(ResolverRule rule);
        void AddFact(ResolverFact fact);
        ResolverResult Resolve();
    }
}
