using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public abstract class Resolver : IResolver
    {
        private bool _resolveFinished = false;

        protected List<ResolverFact> _facts;
        protected List<ResolverRule> _rules;
        protected List<ResolverRule> _usedRules;

        public ReadOnlyCollection<ResolverFact> Facts { get { return _facts.AsReadOnly(); } }
        public ReadOnlyCollection<ResolverRule> Rules { get { return _rules.AsReadOnly(); } }

        public Resolver()
        {
            _facts = new List<ResolverFact>();
            _rules = new List<ResolverRule>();
            _usedRules = new List<ResolverRule>();
        }

        public void AddFact(ResolverFact fact)
        {
            if (!_facts.Contains(fact))
                _facts.Add(fact);
        }

        public void AddRule(ResolverRule rule)
        {
            if (_rules.Contains(rule))
                _rules.Add(rule);
        }

        public virtual ResolverResult Resolve()
        {
            if (_resolveFinished)
                throw new Exception("Нельзя запускать решатель второй раз.");
            _resolveFinished = true;

            return null;
        }

        protected void MarkRuleAsUsed(ResolverRule rule)
        {
            if (!_rules.Contains(rule))
                _usedRules.Add(rule);
        }
    }
}
