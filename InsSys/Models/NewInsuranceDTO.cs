using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsSys.Models
{
    public class NewInsuranceDTO
    {
        //PersonalData
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        public string PESEL { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Street")]
        public string Street { get; set; }
        [Display(Name = "House No")]
        public string HouseNo { get; set; }
        [Display(Name = "Local No")]
        public string LocalNo { get; set; }
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }
        
        //Insurance
        [Display(Name = "Insurer")]
        public string Insurer { get; set; }
        [Display(Name = "Insurance Option")]
        public string Variant { get; set; }
        public int InsurancePackageNo { get; set; }
        public string InsuranceNr { get; set; }
        [Display(Name ="Insurance Start Date")]
        public DateTime InsuranceStartDate { get; set; }
        [Display(Name = "Insurance End Date")]
        public DateTime InsuranceEndDate { get; set; }
        public string Status { get; set; }
        public string LastEditUser { get; set; }
        public DateTime LastEditDate { get; set; }
    }
}