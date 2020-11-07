using InsSys;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcRazorToPdf;
using RazorPDF;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Text.RegularExpressions;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp.Pdf;
using PdfSharp;

namespace InsuranceSystem.Controllers
{
    public class ManageInsurancesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test(int id)
        {
            using(var db = new InsuranceSystemContext())
            {
                var record = db.Insurances.Where(x => x.Id == id)
                    .Include(x => x.IC)
                    .Include(x => x.PersonalData).First();
                return View("Templates/PolicyDocument", record);

            }
        }

        [HttpGet]
        public ActionResult ShowPolicy(int id)
        {
            using(var db = new InsuranceSystemContext())
            {
                var record = db.Insurances.Where(x => x.Id == id)
                    .Include(x => x.IC)
                    .Include(x => x.PersonalData).First();
                return PartialView("Partials/ViewPolicyPartial", record);
            }
        }

        [HttpGet]
        public JsonResult GetInsurances()
        {
            List<InsDTO> dtos = new List<InsDTO>();
            using(var db = new InsuranceSystemContext())
            {
                var records = db.Insurances.Where(x=>x.Status != "DELETED").Include(x=>x.PersonalData).ToList();
                foreach(var record in records)
                {
                    var icid = db.InsurancePackages.Where(x => x.PackageNo == record.InsurancePackageNo).Select(x => x.Id_IC).First();
                    var ICName = db.InsuranceCompanies.Find(icid).ICName;
                    dtos.Add(new InsDTO
                    {
                        id = record.Id,
                        PESEL = record.PersonalData.PESEL,
                        IC = ICName,
                        PackageString = db.InsurancePackages.Where(x => x.PackageNo == record.InsurancePackageNo).First().PackageName,
                        InsStartDate = record.InsuranceStartDate,
                        InsEndDate = record.InsuranceEndDate
                    });
                }
                string dtosjson = JsonConvert.SerializeObject(dtos, new JsonSerializerSettings()
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
                    .Include(x=>x.PersonalData).First();
                string html = RenderRazorViewToString("Templates/PolicyDocument", record);
                html = InlineCss(html);
                ////    fetch view's pdf styles
                string CSSContent = "";
                PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
                pdf.Save("document.pdf");
                return File("document.pdf", "application/pdf", "Test.pdf");
                //StringReader sr = new StringReader(html.ToString());
                //Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(document);
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                //    document.Open();

                //    htmlparser.Parse(sr);
                //    document.Close();

                //    byte[] bytes = memoryStream.ToArray();
                //    memoryStream.Close();
                //    var file = new FileInfo("~/Content/file.pdf");
                //    System.IO.File.WriteAllBytes(@"\\SPPLFAPCEN02.pl.ing-ad\userdata\EH91SA\Documents\PI\InsSys\Content\file.pdf", bytes);
                //    return File(bytes, "application/pdf", "Test.pdf");
                //}
                //var res = new PdfActionResult("Templates/PolicyDocument.cshtml", record);
                //res.FileDownloadName = $"{record.PersonalData.PESEL}_Policy.pdf";
                //return res;
            }
        }



        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        private string InlineCss(string html)
        {
            Match match = null;
            var rx = new Regex(@"<link[^>]*href=""([^h][^t][^t][^p][^""]*.css)""[^>]*>", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            match = rx.Match(html);
            while (match.Success)
            {


                var path = HttpContext.Server.MapPath(match.Groups[1].ToString());
                var content = System.IO.File.ReadAllText(path);


                html = html.Replace(match.ToString(), string.Format(@"<style>{0}</style>", content));
                match = rx.Match(html);
            }
            return html;
        }

        private string InlineImg(string html)
        {
            Match match = null;
            var rx = new Regex(@"<img[^>]*src=""([^d][^a][^t][^a][^:][^""]*)""[^>]*>", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            match = rx.Match(html);
            while (match.Success)
            {
                var path = HttpContext.Server.MapPath(match.Groups[1].ToString());
                var content = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
                html = html.Replace(match.ToString(), string.Format(@"<img src=""data:image/gif;base64,{0}"" />", content));
                match = rx.Match(html);
            }
            return html;
        }

        private string InlineJs(string html)
        {
            Match match = null;
            var rx = new Regex(@"<script[^>]*src=""([^""]*.js)""[^>]*>", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            match = rx.Match(html);
            while (match.Success)
            {
                if (match.Groups[1].ToString().ToLower().Contains("http")) return html;
                var path = HttpContext.Server.MapPath(match.Groups[1].ToString());
                var content = System.IO.File.ReadAllText(path);
                html = html.Replace(match.ToString(), string.Format(@"<script>{0}</script>", content));
                match = rx.Match(html);
            }
            return html;
        }

        static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

    }
}