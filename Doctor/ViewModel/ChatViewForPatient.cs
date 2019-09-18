using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doctor.Models;

namespace Doctor.ViewModel
{
    public class ChatViewForPatient
    {
        public Chat Chat { get; set; }
        public Patient Patient { get; set; }
        public IEnumerable<Doctors> Doctors { get; set; }
    }
}
