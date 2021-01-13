using InsSys.Services.Interfaces;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services
{
    public class SalesResultServices : ISalesResultServices
    {
        private static Dictionary<int, string> MonthName = new Dictionary<int, string> {
            {1, "January" }, {2, "February"}, {3, "March"}, {4, "April"}, {5, "May"}, {6, "June"},
            {7, "July" }, {8, "August"}, {9, "September"}, {10, "October"}, {11, "November"}, {12, "December"}
        };

        public string GetMonthName(int month)
        {
            return MonthName[month];
        }

        public double[] GetInsurancesToPredict(DateTime date2)
        {
            using (var db = new InsuranceSystemContext())
            {
                var date = date2.AddMonths(-5);
                var endDate = date.AddMonths(1).AddDays(-1);
                var salesMonth1 = db.Insurances.Where(x => x.InsuranceStartDate.CompareTo(date) > 0 && x.InsuranceStartDate.CompareTo(endDate) < 0).ToList().Count;
                date = date2.AddMonths(-4);
                endDate = date.AddMonths(1).AddDays(-1);
                var salesMonth2 = db.Insurances.Where(x => x.InsuranceStartDate.CompareTo(date) > 0 && x.InsuranceStartDate.CompareTo(endDate) < 0).ToList().Count;
                date = date2.AddMonths(-3);
                endDate = date.AddMonths(1).AddDays(-1);
                var salesMonth3 = db.Insurances.Where(x => x.InsuranceStartDate.CompareTo(date) > 0 && x.InsuranceStartDate.CompareTo(endDate) < 0).ToList().Count;
                date = date2.AddMonths(-2);
                endDate = date.AddMonths(1).AddDays(-1);
                var salesMonth4 = db.Insurances.Where(x => x.InsuranceStartDate.CompareTo(date) > 0 && x.InsuranceStartDate.CompareTo(endDate) < 0).ToList().Count;
                date = date2.AddMonths(-1);
                endDate = date.AddMonths(1).AddDays(-1);
                var salesMonth5 = db.Insurances.Where(x => x.InsuranceStartDate.CompareTo(date) > 0 && x.InsuranceStartDate.CompareTo(endDate) < 0).ToList().Count;

                double[] lists = { salesMonth1, salesMonth2, salesMonth3, salesMonth4, salesMonth5 };
                return lists;
            }
        }

        public double GetPredictedSale(DateTime date1)
        {
            DateTime date2 = date1.AddYears(-1);
            using (var db = new InsuranceSystemContext())
            {
                var listsYearBefore = GetInsurancesToPredict(date2);
                var listsThisYear = GetInsurancesToPredict(date1);

                double[] divisions = new double[listsThisYear.Length];
                for (int i = 0; i < listsYearBefore.Length; i++)
                {
                    if(listsYearBefore[i] == 0 || listsThisYear[i] == 0)
                    {
                        divisions[i] = 0;
                        continue;
                    }
                    double result = listsYearBefore[i] / listsThisYear[i];
                    divisions[i] = result;
                }
                double sum = divisions.Sum();
                double average = sum / divisions.Length;
                var dateTmp = date2.AddMonths(1).AddDays(-1);
                var monthYearBefore = db.Insurances.Where(x => x.InsuranceStartDate.CompareTo(date2) > 0 && x.InsuranceStartDate.CompareTo(dateTmp) < 0).ToList().Count;
                return average * monthYearBefore;
            }
        }

        public HashSet<SalesNumber> GetSalesSixMonthsBack(DateTime now)
        {
            HashSet<SalesNumber> Sales = new HashSet<SalesNumber>();
            using (var db = new InsuranceSystemContext())
            {
                for (int i = 6; i > 0; i--)
                {
                    var date = now.AddMonths(-1 * (i - 1));
                    var salesCount = db.Insurances.Where(x => x.InsuranceStartDate.Year == date.Year && x.InsuranceStartDate.Month == date.Month).ToList().Count;
                    string monthName = MonthName[date.Month];
                    Sales.Add(new SalesNumber {Label = monthName, Sales = salesCount });
                }
            }
            return Sales;
        }

        public int GetSalesFromMonth(DateTime date1)
        {
            var date = new DateTime(date1.Year, date1.Month, 1);
            using(var db = new InsuranceSystemContext())
            {
                var salesCount = db.Insurances.Where(x => x.InsuranceStartDate.Year == date.Year && x.InsuranceStartDate.Month == date.Month).ToList().Count;
                return salesCount;
            }
        }

        public int PredictedSales(DateTime date1)
        {
            var date = new DateTime(date1.Year, date1.Month, 1);
            double PredictedSalesNumber = GetPredictedSale(date);
            return Convert.ToInt32(Math.Round(PredictedSalesNumber,0));
        }


    }
}