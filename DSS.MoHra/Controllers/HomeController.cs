using DSS.MoHra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            var userSession = db.UserSessions.FirstOrDefault(d => d.UserId == Helpers.Identity.User.Id && !d.IsCompleted);
            if (userSession == null)
            {
                userSession = new UserSession() { UserId = Helpers.Identity.User.Id, IsCompleted = false };
                db.UserSessions.Add(userSession);
                db.SaveChanges();
            }
            var nextQuestionId = db.GetNextQuestion(userSession.Id);
            if (nextQuestionId > 0)
            {
                var question = db.Questions.Include("QuestionVariants").FirstOrDefault(d => d.Id == nextQuestionId);
                model.NextQuestion = question;
            }
            else
            {
                userSession.IsCompleted = true;
                db.SaveChanges();
                return RedirectToAction("Solve", new { id = userSession.Id });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(List<int> questionVariantId)
        {
            var model = new HomeViewModel();
            var userSession = db.UserSessions.FirstOrDefault(d => d.UserId == Helpers.Identity.User.Id && !d.IsCompleted);
            if (userSession != null)
            {
                foreach (var answ in questionVariantId)
                {
                    var variantItem = db.QuestionVariants.FirstOrDefault(d => d.Id == answ);
                    if (variantItem != null)
                        userSession.QuestionVariants.Add(variantItem);
                }
                db.SaveChanges();
                var nextQuestionId = db.GetNextQuestion(userSession.Id);
                if (nextQuestionId > 0)
                {
                    var question = db.Questions.Include("QuestionVariants").FirstOrDefault(d => d.Id == nextQuestionId);
                    model.NextQuestion = question;
                }
                else
                {
                    userSession.IsCompleted = true;
                    db.SaveChanges();
                    return RedirectToAction("Solve", new { id = userSession.Id });
                }
            }

            return View(model);
        }

        public ActionResult Solve(int id)
        {
            var model = new HomeViewModel();

            var resolver = new Resolver.DirectResolver();

            var userSession = db.UserSessions.FirstOrDefault(d => d.UserId == Helpers.Identity.User.Id && d.Id == id);
            var factsFromResults = db.Results.ToList().Select(i => new Resolver.ResolverFact(i.Code, null));
            var factsFromAnswers = userSession.QuestionVariants.ToList().Select(i => new Resolver.ResolverFact(i.FactCode, true));
            
            var answeredQuestionIds = userSession.QuestionVariants.Select(i => i.QuestionId).Distinct();
            var factsNotFromAnswers = db.QuestionVariants
                                        .Where(i => answeredQuestionIds.Contains(i.QuestionId))
                                        .ToList()
                                        .Except(userSession.QuestionVariants.ToList())
                                        //.ToList()
                                        .Select(i => new Resolver.ResolverFact(i.FactCode, false));

            var allFacts = factsFromResults.Union(factsFromAnswers).Union(factsNotFromAnswers).ToList();

            //db.ResolverConditions.Select(i => new Resolver.ResolverRule(i.Premise, ));
            foreach (var fact in allFacts)
                resolver.AddFact(fact);
            foreach (var fact in allFacts.Where(i => i.QuestionValue.HasValue))
                resolver.AddKnownFact(fact);

            //var factName1 = factsFromAnswers.First().Code;
            //var factName2 = factsFromAnswers.Last().Code;
            //var premise = factName1 + "+" + factName2;
            //resolver.AddRule(new MoHra.Resolver.ResolverRule(premise, allFacts.Where(i => !i.QuestionValue.HasValue).First()));

            var rules = db.ResolverConditions.ToList().Select(i => new Resolver.ResolverRule(i.Premise, allFacts.FirstOrDefault(d => d.Code == i.ConclusionResult.Code)));
            foreach (var rule in rules)
                resolver.AddRule(rule);

            var result = resolver.Resolve();
            model.Result = result;

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}