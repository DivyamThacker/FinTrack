using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }//hash the password later
        public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    //public ICollection<UserRole> UserRoles { get; set; } // Navigation property for many-to-many relationship with Roles
    }

}
