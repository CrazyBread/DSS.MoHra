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

        private List<ResolverFact> _facts;
        private List<ResolverFact> _knownFacts;
        private List<ResolverRule> _rules;
        private List<ResolverRule> _usedRules;
        private List<ResolverAnswer> _answers;

        protected ReadOnlyCollection<ResolverFact> KnownFacts { get { return _knownFacts.AsReadOnly(); } }
        protected ReadOnlyCollection<ResolverRule> UsedRules { get { return _usedRules.AsReadOnly(); } }
        public ReadOnlyCollection<ResolverFact> Facts { get { return _facts.AsReadOnly(); } }
        public ReadOnlyCollection<ResolverRule> Rules { get { return _rules.AsReadOnly(); } }
        public ReadOnlyCollection<ResolverAnswer> Answers { get { return _answers.AsReadOnly(); } }

        public Resolver()
        {
            _facts = new List<ResolverFact>();
            _knownFacts = new List<ResolverFact>();
            _rules = new List<ResolverRule>();
            _usedRules = new List<ResolverRule>();
            _answers = new List<ResolverAnswer>();
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

        public void AddAnswer(ResolverAnswer answer)
        {
            if (!_answers.Contains(answer))
                _answers.Add(answer);
        }

        public void DeleteAnswer(ResolverAnswer answer)
        {
            if (_answers.Contains(answer))
                _answers.Remove(answer);
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
            if (!_usedRules.Contains(rule))
                _usedRules.Add(rule);
        }
        
        protected bool _RuleIsWorked(ResolverRule rule)
        {
            if (_usedRules.Contains(rule))
                return false;

            var factNames = rule.Premise.Split(new char[] { '+', '*', '(', ')', '!' }, StringSplitOptions.RemoveEmptyEntries);
            var factItems = _facts.Where(i => factNames.Contains(i.Code));

            if (factNames.Count() != factItems.Count())
                return false;
                //throw new ArgumenException("Не все факты, используемые в правиле, найдены.");

            if (factItems.Except(_knownFacts).Any())
                return false;

            var resultPremise = rule.Premise;
            resultPremise = resultPremise.Replace("+", "||").Replace("*", "&&");
            foreach(var fact in factItems.OrderByDescending(i => i.Code.Length))
            {
                var value = !fact.QuestionValue.HasValue ? "true" : fact.QuestionValue.ToString().ToLower();
                resultPremise = resultPremise.Replace(fact.Code, value);
            }

            return ResolverHelper.Evaluate(resultPremise);
        }
    }
}
