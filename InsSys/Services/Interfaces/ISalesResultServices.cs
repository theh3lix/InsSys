using InsSys.Models;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services.Interfaces
{
    public interface ISalesResultServices
    {

        string GetMonthName(int month);
        double[] GetInsurancesToPredict(DateTime date2);
        double GetPredictedSale(DateTime date1);
        HashSet<SalesNumber> GetSalesSixMonthsBack(DateTime now);
        int GetSalesFromMonth(DateTime date1);
        int PredictedSales(DateTime date1);
    }
}