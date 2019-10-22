using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Medicine Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "MG/ML")]
        public double Amount { get; set; }

        [Required]
        public int Dose { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public string Comment { get; set; }

        public Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }
    }
}
