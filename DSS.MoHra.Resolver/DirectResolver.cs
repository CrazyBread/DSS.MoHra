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
                shouldRepeat = false;
                result.Add("Начался цикл прохождения правил.");
                foreach (var rule in Rules.Except(UsedRules))
                {
                    if (_RuleIsWorked(rule))
                    {
                        MarkRuleAsUsed(rule);
                        shouldRepeat = true;
                        AddKnownFact(rule.Conclusion);
                        result.Add("Добавлен новый факт " + rule.Conclusion.Code + ".");
                    }
                }
            } while (shouldRepeat);

            result.Facts.AddRange(KnownFacts.Where(i => !i.QuestionValue.HasValue));

            return result;
        }
    }
}
