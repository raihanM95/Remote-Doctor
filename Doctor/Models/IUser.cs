using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Models
{
    public interface IUser
    {
        // Property
        string Name { get; set; }
        string Gender { get; set; }
        DateTime DateOfBirth { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string ImagePath { get; set; }
        string Address { get; set; }
        string Password { get; set; }
    }
}
