using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Upload Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        [Required]
        public HttpPostedFileBase Imagefile { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(200)]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        public string Password { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm Password")]
        //[MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        //[Compare("Password", ErrorMessage = "Password doesn't match")]
        //[NotMapped]
        //public string ConfirmPassword { get; set; }

        public bool IsEmailVarified { get; set; }

        public Guid ActivationCode { get; set; }
    }
}
