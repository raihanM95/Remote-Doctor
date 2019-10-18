using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Test Name")]
        public string TestName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Upload Report file")]
        public string ImagePath { get; set; }

        [NotMapped]
        ///[Required]
        public HttpPostedFileBase Imagefile { get; set; }

        public virtual Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }
    }
}
