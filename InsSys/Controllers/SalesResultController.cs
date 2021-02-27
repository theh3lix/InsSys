using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using InsSys.Models;
using InsSys.Services;
using Newtonsoft.Json;

namespace InsSys.Controllers
{
    
    public class SalesResultController : Controller
    {
        private static SalesResultServices services = new SalesResultServices();
        public ActionResult Index() => View();
        
        [HttpGet]
        public ActionResult GetSalesSixMonthsBack()
        {
            HashSet<SalesNumber> Sales = new HashSet<SalesNumber>();
            var now = DateTime.Now;
            Sales = services.GetSalesSixMonthsBack(now);
            string json = JsonConvert.SerializeObject(Sales);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSalesSixMonthsYearBefore()
        {
            HashSet<SalesNumber> Sales = new HashSet<SalesNumber>();
            var now = DateTime.Now.AddYears(-1);
            Sales = services.GetSalesSixMonthsBack(now);
            string json = JsonConvert.SerializeObject(Sales);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}