using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsSys.Models
{
    public class PersonalData
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PESEL { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string LocalNo { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
    }
}