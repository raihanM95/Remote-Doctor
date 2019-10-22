using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Doctors
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Enter Name")]
        public string DoctorName { get; set; }

        [Display(Name = "Upload Image")]
        public string DoctorImagePath { get; set; }

        [NotMapped]
        //[Required]
        public HttpPostedFileBase DoctorImagefile { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date of Birth")]
        public DateTime DoctorBirthDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string DoctorEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Degree")]
        [StringLength(200)]
        public string DoctorDegree { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Registration No")]
        public string RegNo { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        [Display(Name = "Details")]
        public string DoctorDetails { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        [Display(Name = "Available time")]
        public string StartTime { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        //[Display(Name = "Available time")]
        public string EndTime { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(200)]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        public string DoctorPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        [Compare("DoctorPassword", ErrorMessage = "Password doesn't match")]
        [NotMapped]
        public string DoctorConfirmPassword { get; set; }

        public bool IsEmailVarified { get; set; }

        public Guid ActivationCode { get; set; }

        public Department Department { get; set; }
        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
    }
}
