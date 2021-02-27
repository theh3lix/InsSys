using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsSys.Models
{
    [Table("userrole")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string Role_name { get; set; }
    }
}