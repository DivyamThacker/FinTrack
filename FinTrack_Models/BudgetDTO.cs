using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace FinTrack_Models
{
    public class BudgetDTO
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter name..")]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required]
        public string? Period { get; set; }
        [Required]
        public int Amount { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
        public bool Notify { get; set; }

        public int DailySpentAmount { get; set; }//average of total spent amount till now and the days passed

        public int TotalSpentAmount { get; set;}//till now for the particular category and timing

        public int DailyRecommendedAmount { get; set; } //to not exceed the daily budget

        public DateTime EstimatedDate { get; set; } //for the budget money to run out
    }
}
