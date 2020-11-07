using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceSystem.Models
{
    public class AuthorizedUser
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public int Id_role { get; set; }
    }
}