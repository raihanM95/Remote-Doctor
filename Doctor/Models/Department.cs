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
        [Display(Name = "Department name")]
        public string DeptName { get; set; }

        [Display(Name = "Department details")]
        public string DeptDetails { get; set; }
    }
}
