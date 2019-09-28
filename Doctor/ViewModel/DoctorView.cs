using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor.ViewModel
{
    public class DoctorView
    {
        public IEnumerable<Department> Departments { get; set; }
        public Doctors Doctors { get; set; }
        public IEnumerable<Doctors> Doctorses { get; set; }
    }
}
