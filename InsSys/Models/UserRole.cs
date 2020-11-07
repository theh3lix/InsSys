using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceSystem.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string Role_name { get; set; }
    }
}