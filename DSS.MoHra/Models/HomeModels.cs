using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSS.MoHra.Models
{
    public class HomeViewModel
    {
        public Question NextQuestion;
        public Resolver.ResolverResult Result;
        public List<Resolver.ResolverFact> Answers;
        public List<ResolverCondition> Conditions;
        public List<Result> Results;
    }
}