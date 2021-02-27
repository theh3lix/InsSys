using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsSys.Services;
using InsSys.Services.Interfaces;
using InsSys.Models;

namespace InsSys.Controllers
{
    public class NewInsuranceController : Controller
    {
        INewInsuranceServices services = new NewInsuranceServices();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterInsurance(NewInsuranceDTO newInsurance)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            IInsuranceBuilder builder = new InsuranceBuilder();
            builder.BuildBasicInsuranceInfo(newInsurance);
            builder.BuildPersonalData(newInsurance);
            builder.BuildFinalInsurance();
            Insurance ins = builder.GetProduct();

            using(var db = new InsuranceSystemContext())
            {
                db.Entry(ins).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { controller="ManageInsurances" });
        }

    }
    
    
}