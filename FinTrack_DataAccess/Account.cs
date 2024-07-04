using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class Account
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public decimal Balance { get; set; } = 0;
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

