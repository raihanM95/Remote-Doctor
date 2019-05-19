using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RemoteDoctor.Models
{
    public class Admin
    {
        //[Required(ErrorMessage = "Please enter your id")]
        [Display(Name = "Admin Id")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}