using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor.ViewModel
{
    public class PrescriptonView
    {
        public IEnumerable<Prescription> Prescriptions { get; set; }
        public IEnumerable<Medicine> Medicines{ get; set; }
        public IEnumerable<Report> Reports{ get; set; }
    }
}
