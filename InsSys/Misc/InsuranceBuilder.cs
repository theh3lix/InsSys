using InsSys.Services.Interfaces;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services
{
    public class InsuranceBuilder : IInsuranceBuilder
    {
        private static INewInsuranceServices services = new NewInsuranceServices();
        private Insurance Insurance = new Insurance();
        public InsuranceBuilder()
        {
            this.ResetBuilder();
        }
        public void ResetBuilder()
        {
            this.Insurance = new Insurance();
        }


        public void BuildBasicInsuranceInfo(NewInsuranceDTO dto)
        {
            Insurance ins = services.GetInsuranceFromDTO(dto);
            this.Insurance = ins;
        }

        public void BuildPersonalData(NewInsuranceDTO dto)
        {
            PersonalData pd = services.GetPersonalDataFromDTO(dto);
            this.Insurance.PersonalData = pd;
        }

        public void BuildFinalInsurance()
        {
            this.Insurance = services.FillInsurance(Insurance);
        }

        public Insurance GetProduct()
        {
            Insurance finalInsurance = this.Insurance;
            this.ResetBuilder();
            return finalInsurance;
        }

    }
}