using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class AccountCreationRequestDTO
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string UserId { get; set; }
    }

}
