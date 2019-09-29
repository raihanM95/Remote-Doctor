using Doctor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doctor.ViewModel
{
    public class CCordinatorView
    {
        public CCordinator CCordinator { get; set; }
        public IEnumerable<CCordinator> CCordinators { get; set; }
    }
}
