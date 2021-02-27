using InsSys.Models;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services.Interfaces
{
    public interface INewInsuranceServices
    {

        PersonalData GetPersonalDataFromDTO(NewInsuranceDTO dto);
        Insurance GetInsuranceFromDTO(NewInsuranceDTO dto);
        List<InsuranceCompany> GetInsurers();
        Insurance FillInsurance(Insurance InsuranceRecord);
    }
}