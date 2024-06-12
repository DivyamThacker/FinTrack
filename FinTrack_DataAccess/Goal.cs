using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class Goal
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Required]
        public string? Period { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string? Status { get; set; }
        public string? Category { get; set; }
        public bool Notify { get; set; }
    }
}
