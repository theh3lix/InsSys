using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsSys.Services;
using InsuranceSystem.Models;

namespace InsSys.Controllers
{
    public class NewInsuranceController : Controller
    {
        NewInsuranceServices services = new NewInsuranceServices();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterInsurance(NewInsuranceDTO newInsurance)
        {
            PersonalData Client = services.GetPersonalDataFromDTO(newInsurance);
            Insurance InsuranceRecord = services.GetInsuranceFromDTO(newInsurance);
            using(var db = new InsuranceSystemContext())
            {
                string lastInsuranceNo = db.Insurances.ToList().OrderByDescending(x => x.Id).First().InsuranceNr;
                long newInsuranceNo = Convert.ToInt64(lastInsuranceNo) + 1;
                var id_ic = db.InsurancePackages.Where(x => x.PackageNo == InsuranceRecord.InsurancePackageNo).First().Id_IC;
                var ic = db.InsuranceCompanies.Find(id_ic);
                InsuranceRecord.PersonalData = Client;
                InsuranceRecord.IC = ic;
                InsuranceRecord.InsuranceNr = newInsuranceNo.ToString();
                db.Entry(InsuranceRecord).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new {controller="ManageInsurances" });
        }
    }
    
    
}