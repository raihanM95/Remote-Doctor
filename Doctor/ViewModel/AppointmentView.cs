 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor.ViewModel
{
    public class AppointmentView
    {
        public Doctors Doctorses { get; set; }
        public Appointment Appointment { get; set; }
        public Patient Patient { get; set; }
    }
}
