using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public class DirectResolver : Resolver
    {
        public override ResolverResult Resolve()
        {
            base.Resolve();

            var result = new ResolverResult();

            bool shouldRepeat = false;
            do
            {
                result.Add("Начался цикл прохождения правил.");
                foreach (var rule in _rules.Except(_usedRules))
                {
                    if (_RuleIsWorked(rule))
                    {
                        _usedRules.Add(rule);
                        shouldRepeat = true;
                        AddKnownFact(rule.Conclusion);
                        result.Add("Добавлен новый факт " + rule.Conclusion.Code + ".");
                    }
                }
            } while (shouldRepeat);

            return result;
        }
    }
}
