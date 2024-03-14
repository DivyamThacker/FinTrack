using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class Record
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime RecordDate { get; set; }
        public int Amount { get; set; }
        public bool IsIncome { get; set; }
        public string? Category { get; set; }
    }
}
