using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Advice { get; set; }

        public virtual Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }
    }
}
