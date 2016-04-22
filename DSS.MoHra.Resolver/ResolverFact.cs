using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public class ResolverFact
    {
        public string Code { protected set; get; }
        public bool? QuestionValue { protected set; get; }

        public ResolverFact(string code, bool? questionValue = null)
        {
            Code = code;
            QuestionValue = questionValue;
        }
    }
}
