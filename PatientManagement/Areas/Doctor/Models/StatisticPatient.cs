using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientManagement.Areas.Doctor.Models
{
    public class StatisticPatient
    {
        public string month { get; set; }
        public int year { get; set; }
        public int count { get; set; }
    }
}