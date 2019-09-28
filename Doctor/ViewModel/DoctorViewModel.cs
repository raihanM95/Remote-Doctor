using Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.ViewModel
{
    public class DoctorViewModel
    {
        public IEnumerable<Doctors> Data { get; set; }
    }
}