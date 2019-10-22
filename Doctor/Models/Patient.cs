using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Enter Name")]
        public string PatientName { get; set; }

        [Display(Name = "Upload Image")]
        public string PatientImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase PatientImagefile { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date of Birth")]
        public DateTime? PatientBirthDate { get; set; }

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string PatientPhone { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string PatientEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(200)]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        public string PatientPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        [Compare("PatientPassword", ErrorMessage = "Password doesn't match")]
        [NotMapped]
        public string PatientConfirmPassword { get; set; }

        public bool IsEmailVarified { get; set; }

        public Guid ActivationCode { get; set; }
    }
}
