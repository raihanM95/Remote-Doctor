using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        [Display(Name = "Running any medicine?")]
        public bool RunningMedicine { get; set; }

        [Display(Name = "Your Blood Pressure")]
        public string BP { get; set; }

        //[Required(AllowEmptyStrings = false)]
        [Display(Name = "Your problem description")]
        public string Problem { get; set; }

        //[Required(AllowEmptyStrings = false)]
        [Display(Name = ("Your Weight"))]
        public string Weight { get; set; }

        public bool Status { get; set; }

        public bool AcceptStatus { get; set; }

        [Display(Name = "Appointment Time")]
        [StringLength(20)]
        public string AppointmentTime { get; set; }

        [Display(Name = "Appointment Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime AppointmentDate { get; set; }

        [DataType(DataType.EmailAddress)]
        public string CCEmail { get; set; }

        public virtual Doctors Doctors { get; set; }
        public int DoctorsId { get; set; }
        public virtual Patient Patient { get; set; }
        public int PatientId { get; set; }

        [NotMapped]
        public virtual Medicine Medicine { get; set; }
    }
}
