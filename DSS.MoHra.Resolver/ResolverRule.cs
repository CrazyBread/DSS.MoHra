using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public class ResolverRule
    {
        public string Premise { protected set; get; }
        public ResolverFact Conclusion { protected set; get; }

        public ResolverRule(string premise, ResolverFact conclusion)
        {
            Premise = premise;
            Conclusion = conclusion;
        }
    }
}
