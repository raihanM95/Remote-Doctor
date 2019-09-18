using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor.ViewModel
{
    public class ChatViewForDoctor
    {
        public Chat Chat { get; set; }
        public Doctors Doctors { get; set; }
        public IEnumerable<Patient> Patient { get; set; }
    }
}
