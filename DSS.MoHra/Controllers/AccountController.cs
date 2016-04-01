using DSS.MoHra.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSS.MoHra.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                int userId = -1;
                if (IdentityManager.IsCreditinalsCorrect(model.Login, model.Password, ref userId))
                {
                    var claimsIdentity = IdentityManager.GenerateIdentity(userId, model.Login);
                    var authenticationManager = Request.GetOwinContext().Authentication;
                    authenticationManager.SignOut();
                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.IsPersistent }, claimsIdentity);

                    // update date of last login
                    var user = db.Users.First(i => i.Id == userId);
                    user.DateLastLogin = DateTime.Now;
                    db.SaveChanges();

                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                ModelState.AddModelError("", "Такие учётные данные не найдены. Пожалуйста, повторите попытку.");
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout(string returnUrl)
        {
            var authenticationManager = Request.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }
    }
}