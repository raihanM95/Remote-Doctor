using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DeptName { get; set; }

        public string DeptDetails { get; set; }
    }
}
