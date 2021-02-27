using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsSys.Models
{
    [Table("insurancecompany")]
    public class InsuranceCompany
    {
        [Key]
        public int Id { get; set; }
        public string ICName { get; set; }
        public bool Active { get; set; }
    }
}