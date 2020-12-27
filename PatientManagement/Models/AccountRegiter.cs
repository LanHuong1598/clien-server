using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientManagement.Models
{
    public class AccountRegiter
    {
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string UserName { get; set; }

        public bool Gender { get; set; }

        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }


        public string Email { get; set; }

        public string IndentificationCardId { get; set; }

        public DateTime IndentificationCardDate { get; set; }
    }
}