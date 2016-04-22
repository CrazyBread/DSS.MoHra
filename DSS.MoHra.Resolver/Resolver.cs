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
        protected List<ResolverFact> _knownFacts;
        protected List<ResolverRule> _rules;
        protected List<ResolverRule> _usedRules;

        public ReadOnlyCollection<ResolverFact> Facts { get { return _facts.AsReadOnly(); } }
        public ReadOnlyCollection<ResolverRule> Rules { get { return _rules.AsReadOnly(); } }

        public Resolver()
        {
            _facts = new List<ResolverFact>();
            _knownFacts = new List<ResolverFact>();
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
            if (!_rules.Contains(rule))
                _rules.Add(rule);
        }

        public virtual ResolverResult Resolve()
        {
            if (_resolveFinished)
                throw new Exception("Нельзя запускать решатель второй раз.");
            _resolveFinished = true;

            return null;
        }

        public void AddKnownFact(ResolverFact fact)
        {
            if (!_knownFacts.Contains(fact))
                _knownFacts.Add(fact);
        }

        protected void MarkRuleAsUsed(ResolverRule rule)
        {
            if (!_rules.Contains(rule))
                _usedRules.Add(rule);
        }
        
        protected bool _RuleIsWorked(ResolverRule rule)
        {
            var factNames = rule.Premise.Split(new char[] { '+', '*', '(', ')', '!' }, StringSplitOptions.RemoveEmptyEntries);
            var factItems = _facts.Where(i => factNames.Contains(i.Code));

            if (factNames.Count() != factItems.Count())
                throw new ArgumentException("Не все факты, используемые в правиле, найдены.");

            if (factItems.Except(_knownFacts).Any())
                return false;

            var resultPremise = rule.Premise;
            foreach(var fact in factItems.OrderByDescending(i => i.Code.Length))
            {
                var value = !fact.QuestionValue.HasValue ? "1" : Convert.ToInt16(fact.QuestionValue).ToString();
                resultPremise = resultPremise.Replace(fact.Code, value);
            }

            throw new NotImplementedException();
        }
    }
}
