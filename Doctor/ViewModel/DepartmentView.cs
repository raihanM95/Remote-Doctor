using Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.ViewModel
{
    public class DepartmentView
    {
        public Department Department { get; set; }
        public IEnumerable<Department> Departments { get; set; }
    }
}
