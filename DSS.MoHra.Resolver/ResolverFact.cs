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
        public string Name { get; set; }

        public ResolverFact(string code, string name = "", bool? questionValue = null)
        {
            Code = code;
            Name = name;
            QuestionValue = questionValue;
        }
    }
}
