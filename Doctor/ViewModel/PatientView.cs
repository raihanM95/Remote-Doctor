using Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.ViewModel
{
    public class PatientView
    {
        public Patient Patient { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
    }
}
