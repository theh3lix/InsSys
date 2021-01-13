using InsSys.Models;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services.Interfaces
{
    public interface IInsuranceBuilder
    {

        void ResetBuilder();
        void BuildBasicInsuranceInfo(NewInsuranceDTO dto);
        void BuildPersonalData(NewInsuranceDTO dto);
        void BuildFinalInsurance();
        Insurance GetProduct();
    }
}