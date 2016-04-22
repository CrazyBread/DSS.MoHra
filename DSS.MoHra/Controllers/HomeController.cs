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
            }

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}