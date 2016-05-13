using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public class ReverseResolver : Resolver
    {

        public override ResolverResult Resolve()
        {
            base.Resolve();

            var result = new ResolverResult();
            var summary = "";

            bool shouldRepeat = false;
            do
            {
                shouldRepeat = false;

                var currentAnswer = Answers.Last();
                result.Add("Запущен новый цикл поиска ответа. Ищем факт " + currentAnswer.fact.Code + ": " + currentAnswer.fact.Name);
                var rule = Rules.FirstOrDefault(m => m.Conclusion == currentAnswer.fact);
                result.Add("Выбрано правило " + rule.Premise + " -> " + rule.Conclusion.Code + ": " + rule.Description);
                MarkRuleAsUsed(rule);

                var factNames = rule.Premise.Split(new char[] { '+', '*', '(', ')', '!' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var knowFactItems = KnownFacts.Select(m => m.Code).Where(i => factNames.Contains(i));

                if (factNames.Count() != knowFactItems.Count())
                {
                    result.Add("В правиле имеются неизвестные факты");
                    var list = factNames.Except(knowFactItems);
                    if (list.Count() > 0)
                    {
                        var needFact = Facts.First(m => m.Code == list.First());
                        AddAnswer(new ResolverAnswer(needFact));
                        result.Add("Необходимо определить факт " + needFact.Code + ": " + needFact.Name);
                    }
                } else
                {
                    result.Add("В правиле все факты известны");
                    AddKnownFact(rule.Conclusion);
                    DeleteAnswer(currentAnswer);
                    result.Add("Делаем вывод, что факт " + rule.Conclusion.Code + " известен: " + rule.Conclusion.Name);
                    summary += rule.Conclusion.Name + "\n";
                }
                if (Answers.Count() > 0) {
                    shouldRepeat = true;
                } 
            } while (shouldRepeat);

            result.Facts.AddRange(KnownFacts.Where(i => !i.QuestionValue.HasValue));
            result.Summary = summary;
            return result;
        }
    }
}
