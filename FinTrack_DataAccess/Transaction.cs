using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccountId { get; set; } // Foreign key to Account model
        public SavingsAccount SavingsAccount { get; set; }
        [Required]
        public string SenderUsername { get; set; } = default!;
        [Required]
        public string RecieverUsername { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        public bool IsUserSender { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Category { get; set; }
        public int Amount { get; set; }
    }
}
