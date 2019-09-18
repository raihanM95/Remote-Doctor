using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Doctor.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Message Text")]
        public string MessageText { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Sender { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Reciver { get; set; }

        public DateTime time { get; set; }
    }
}
