using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class AccountCreationResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccountId { get; set; } // Optional: Include the ID of the created account if needed
    }

}
