using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;

namespace InsSys.Controllers
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
                var insurances = db.Insurances.Where(x=>x.Status == "AKTYWNY").ToHashSet();
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

        public ActionResult DropTheBase()
        {
            string path = Server.MapPath("~/Content/drop.xlsx");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            FileInfo file = new FileInfo(path);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using(var package = new ExcelPackage(file))
            using(var db = new InsuranceSystemContext())
            {
                var sheet = package.Workbook.Worksheets.Add("Sheet1");
                var records = db.Insurances.ToHashSet();
                sheet.Cells[2, 1].LoadFromCollection(records);
                package.Save();
            }
            return File(path, "application/xlsx", "dropTheBase.xlsx");
        }

    }
}