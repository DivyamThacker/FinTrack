using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class GoalDTO
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required]
        public string? Period { get; set; }
        [Required]
        public int GoalAmount { get; set; }
        public int TotalSavedAmount { get; set; }//same as total Icome amount till now minus expenses (for now)
        [Required]
        public string? Status { get; set; }
        public string? Category { get; set; }

        public bool Notify { get; set; }

        public int DailySavedAmount { get; set; }//average of total saved amount till now and the days passed

        public int DailyRecommendedAmount { get; set; } //to reach the daily goal

        public DateTime EstimatedDate { get; set; } //to reach the goal amount

        public int AmountSavedLastWeek { get; set; } //to track the progress
    }
}
