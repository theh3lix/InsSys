using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsSys.Models
{
    public class InsuranceCompany
    {
        [Key]
        public int Id { get; set; }
        public string ICName { get; set; }
        public bool Active { get; set; }
    }
}