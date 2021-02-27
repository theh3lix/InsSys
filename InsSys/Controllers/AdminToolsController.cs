using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Web.Mvc;
using System.IO;
using InsSys.Models;
using InsSys.Services;
using InsSys.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Web;

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
                } catch(Exception)
                {
                    return Json("500");
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


        [HttpPost]
        public ActionResult AddAdmin(string login)
        {
            using(var db = new InsuranceSystemContext())
            {
                var user = db.AuthorizedUsers.Where(x => x.Login == login).First();
                if(user != null)
                {
                    user.Id_role = 1;
                    db.Entry(user).State = EntityState.Modified;
                }
                else
                {
                    AuthorizedUser newUser = new AuthorizedUser
                    {
                        Login = login,
                        Id_role = 1
                    };
                    db.Entry(newUser).State = EntityState.Added;
                }
                db.SaveChanges();
            }
            return Json("200");
        }


        [HttpPost]
        public ActionResult ImportInsurances(HttpPostedFileBase fileIn)
        {
            IInsuranceBuilder builder = new InsuranceBuilder();
            string path = Server.MapPath("~/Content/FileIn.xlsx");
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            fileIn.SaveAs(path);
            FileInfo file = new FileInfo((path));
            int row = 2;
            using (var package = new ExcelPackage(file))
            using(var db = new InsuranceSystemContext())
            {
                var sheet = package.Workbook.Worksheets.First();
                while (sheet.Cells[row, 1].Value != null)
                {
                    NewInsuranceDTO insDTO = new NewInsuranceDTO();
                    insDTO.FirstName = sheet.Cells[row, 1].Value.ToString();
                    insDTO.LastName = sheet.Cells[row, 2].Value.ToString();
                    insDTO.DateOfBirth = DateTime.Parse(sheet.Cells[row, 3].Value.ToString());
                    insDTO.PESEL = sheet.Cells[row, 4].Value.ToString();
                    insDTO.Street = sheet.Cells[row, 5].Value.ToString();
                    insDTO.HouseNo = sheet.Cells[row, 6].Value.ToString();
                    insDTO.LocalNo = sheet.Cells[row, 7].Value.ToString();
                    insDTO.PostalCode = sheet.Cells[row, 8].Value.ToString();
                    insDTO.City = sheet.Cells[row, 9].Value.ToString();
                    insDTO.InsuranceStartDate = DateTime.Parse(sheet.Cells[row, 10].Value.ToString());
                    insDTO.InsuranceEndDate = DateTime.Parse(sheet.Cells[row, 11].Value.ToString());
                    var IC = db.InsuranceCompanies.First(x => x.ICName == sheet.Cells[row, 12].Value.ToString());
                    insDTO.Insurer = IC == null ? "0" : IC.Id.ToString();
                    string variant = sheet.Cells[row,13].Value.ToString().ToLower().Contains("podst") ? "1" : "2";
                    var variants = db.InsurancePackages.Where(x => x.Id_IC == IC.Id).ToHashSet();
                    switch (variant)
                    {
                        case "1":
                            insDTO.InsurancePackageNo = variants.First().PackageNo;
                            break;
                        case "2":
                            insDTO.InsurancePackageNo = variants.Last().PackageNo;
                            break;
                    }
                    builder.BuildBasicInsuranceInfo(insDTO);
                    builder.BuildPersonalData(insDTO);
                    builder.BuildFinalInsurance();
                    Insurance ins = builder.GetProduct();
                    db.Entry(ins).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", new { Controller = "ManageInsurances" });
        }
        
    }
}