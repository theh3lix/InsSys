using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using Rotativa;
using InsSys.Services.Interfaces;
using InsSys.Services;

namespace InsSys.Controllers
{
    public class ManageInsurancesController : Controller
    {
        private IManageInsurancesServices services = new ManageInsurancesServices();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowPolicy(int id)
        {
            var record = services.GetRecord(id);
            return PartialView("Partials/ViewPolicyPartial", record);
        }

        [HttpGet]
        public JsonResult GetInsurances()
        {
            var dtos = new List<InsDTO>();
            using(var db = new InsuranceSystemContext())
            {
                var records = db.Insurances.Where(x=>x.Status != "DELETED").Include(x=>x.PersonalData).OrderByDescending(x=>x.InsuranceStartDate).ToHashSet();
                foreach(var record in records)
                {
                    var icid = db.InsurancePackages.Where(x => x.PackageNo == record.InsurancePackageNo).Select(x => x.Id_IC).First();
                    var icName = db.InsuranceCompanies.Find(icid)?.ICName;
                    var packageName = db.InsurancePackages.Where(x => x.PackageNo == record.InsurancePackageNo)?.First().PackageName;
                    dtos.Add(services.GetInsDTO(record, icName, packageName));
                }
                var dtosjson = JsonConvert.SerializeObject(dtos, new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy-MM-dd"
                });
                return Json(dtosjson, JsonRequestBehavior.AllowGet);
            }
        }

        

        [HttpPost]
        public ActionResult DeletePolicy(int id)
        {
            using(var db = new InsuranceSystemContext())
            {
                var record = db.Insurances.Find(id);
                if (record == null)
                    return Json("404");
                record.Status = "DELETED";
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return Json("200");
            }
        }

        [HttpPost]
        public ActionResult GeneratePolicyDocument(int id)
        {
            using (var db = new InsuranceSystemContext())
            {
                var record = db.Insurances.Where(x=>x.Id==id)
                    .Include(x=>x.IC)
                    .Include(x=>x.PersonalData).First(); //Getting full record details to generate policy
                if (record == null)
                    return Json("404");
                return new ViewAsPdf("Templates/PolicyDocument", record);
            }
        }

    }
}