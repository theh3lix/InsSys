using InsSys.Models;
using InsSys.Services.Interfaces;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InsSys.Services
{
    public class ManageInsurancesServices : IManageInsurancesServices
    {

        public InsDTO GetInsDTO(Insurance record, string ICName, string PackageName)
        {
            var dto = new InsDTO
            {
                id = record.Id,
                PESEL = record.PersonalData.PESEL,
                IC = ICName,
                PackageString = PackageName,
                InsStartDate = record.InsuranceStartDate,
                InsEndDate = record.InsuranceEndDate
            };
            return dto;
        }

        public Insurance GetRecord(int id)
        {
            using (var db = new InsuranceSystemContext())
            {
                var record = db.Insurances.Where(x => x.Id == id)
                    .Include(x => x.IC)
                    .Include(x => x.PersonalData).First();
                return record;
            }
        }


    }
}