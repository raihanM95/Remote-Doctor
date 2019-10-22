using Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.ViewModel
{
    public class ReferView
    {
        public IEnumerable<Doctors> Doctors { get; set; }
        public Appointment Appointment { get; set; }
    }
}