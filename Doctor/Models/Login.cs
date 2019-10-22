using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    /// <summary>
    /// The login.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether remember.
        /// </summary>
        [Display(Name = "Remember me")]
        public bool Remember { get; set; }
    }
}
