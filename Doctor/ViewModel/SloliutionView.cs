using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor.ViewModel
{
    public class SloliutionView
    {
        public Appointment Appointment { get; set; }
        public Prescription Prescription { get; set; }
        public Medicine Medicine { get; set; }
        public Report Report { get; set; }
    }
}
