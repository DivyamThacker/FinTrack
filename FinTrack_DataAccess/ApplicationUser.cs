using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        //public string AccountId { get; set; } // Foreign key to Account model
        public ICollection<Account> Accounts { get; set; } // Each user can have multiple accounts
    }
}
