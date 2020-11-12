using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsSys.Models
{
    public class InsDTO
    {
        public int id { get; set; }
        public string PESEL { get; set; }
        public string IC { get; set; }
        public string PackageString { get; set; }
        public DateTime InsStartDate { get; set; }
        public DateTime InsEndDate { get; set; }
        
    }
}