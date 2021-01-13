using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsSys.Models
{
    public class InsurancePackage
    {
        [Key]
        public int Id { get; set; }
        public string PackageName { get; set; }
        public int PackageNo { get; set; }
        public int Id_IC { get; set; }
        public double ContributionSum { get; set; }
    }
}