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
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether remember.
        /// </summary>
        [Display(Name = "Remember me")]
        public bool Remember { get; set; }
    }
}
