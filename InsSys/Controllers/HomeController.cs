using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuranceSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = HttpContext.User.Identity.Name;
            return View();
        }

    }
}