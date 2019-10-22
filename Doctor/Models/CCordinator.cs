using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class CCordinator
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Enter Name")]
        public string CCordinatorName { get; set; }

        [Display(Name = "Upload Image")]
        public string CCordinatorImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase CCordinatorImagefile { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No")]
        public string CCordinatorPhone { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string CCordinatorEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(200)]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        public string CCordinatorPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        [Compare("CCordinatorPassword", ErrorMessage = "Password doesn't match")]
        [NotMapped]
        public string CCordinatorConfirmPassword { get; set; }

        public bool IsEmailVarified { get; set; }

        public Guid ActivationCode { get; set; }
    }
}
