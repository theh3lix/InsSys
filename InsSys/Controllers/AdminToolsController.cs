using InsSys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Web.Mvc;

namespace InsuranceSystem.Controllers
{
    public class AdminToolsController : Controller
    {
        public ActionResult Index()
        {
            var user = HttpContext.User.Identity.Name;
            return View();
        }

        public ActionResult DeleteAllInsurances()
        {
            using(var db = new InsuranceSystemContext())
            {
                var insurances = db.Insurances.Where(x=>x.Status == "AKTYWNY").ToList();
                foreach(var insurance in insurances)
                {
                    insurance.Status = "DELETED";
                    db.Entry(insurance).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return Json("Insurances table has been cleared");
            }
        }

        public ActionResult ClearTable()
        {
            string query = "DELETE FROM Insurances";
            string connString = ConfigurationManager.ConnectionStrings["DB"].ToString();
            using(var conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Execute(query);
                } catch(Exception ex)
                {

                }
            }
            return Json("200");
        }

    }
}