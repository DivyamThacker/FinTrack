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
        public int UserId { get; set; }
        [Required]
        public string GiverUserName { get; set; } = default!;
        [Required]
        public string TakerUserName { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Amount { get; set; }
    }
}
