using InsuranceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services
{
    public class NewInsuranceServices
    {
        public NewInsuranceServices()
        {

        }

        public PersonalData GetPersonalDataFromDTO(NewInsuranceDTO dto)
        {
            return new PersonalData
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                PESEL = dto.PESEL,
                City = dto.City,
                Street = dto.Street,
                HouseNo = dto.HouseNo,
                LocalNo = dto.LocalNo,
                PostalCode = dto.PostalCode,

            };
        }

        public Insurance GetInsuranceFromDTO(NewInsuranceDTO dto)
        {
            int insurerId = Convert.ToInt32(dto.Insurer);
            var returnInsurance = new Insurance
            {
                InsuranceStartDate = dto.InsuranceStartDate,
                InsuranceEndDate = dto.InsuranceEndDate, 
                Status = "AKTYWNY"
            };
            using(var db = new InsuranceSystemContext())
            {
                switch (dto.Variant)
                {
                    case "1":
                        returnInsurance.InsurancePackageNo = db.InsurancePackages.Where(x => x.Id_IC == insurerId).First().PackageNo;
                        break;
                    case "2":
                        returnInsurance.InsurancePackageNo = db.InsurancePackages.Where(x => x.Id_IC == insurerId).OrderByDescending(x=>x.Id).First().PackageNo;
                        break;
                    default:
                        returnInsurance.InsurancePackageNo = 0;
                        break;
                }
            }


            return returnInsurance;
        }

        public static List<InsuranceCompany> GetInsurers()
        {
            using(var db = new InsuranceSystemContext())
            {
                return db.InsuranceCompanies.ToList();
            }
        }

    }
}