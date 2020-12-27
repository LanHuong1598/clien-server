using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientManagement.Areas.Admin.Models
{
    public class StatisticPatient
    {
        public string month { get; set; }
        public int year { get; set; }
        public int count { get; set; }
    }
}