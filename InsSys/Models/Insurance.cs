using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceSystem.Models
{
    public class Insurance
    {
        [Key]
        public int Id { get; set; }
        public int Id_client { get; set; }
        /// <summary>
        /// Dane klienta
        /// </summary>
        [ForeignKey(nameof(Id_client))]
        public PersonalData PersonalData { get; set; }

        public int InsurancePackageNo { get; set; }
        public string InsuranceNr { get; set; }
        public DateTime InsuranceStartDate { get; set; }
        public DateTime InsuranceEndDate { get; set; }
        public string Status { get; set; }
        public string LastEditUser { get; set; }
        public DateTime LastEditDate { get; set; }
        public int id_insuranceCompany { get; set; }

        [ForeignKey(nameof(id_insuranceCompany))]
        public InsuranceCompany IC { get; set; }
    }
}