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

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}