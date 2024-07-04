using FinTrack_Common;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace FinTrack_Models
{
    public class BudgetDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [Required]
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } // Foreign key to Account model
        [Required(ErrorMessage = "Please enter name..")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }
        [Required]
        [JsonPropertyName("period")]
        public string? Period { get; set; }
        [Required]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        [JsonPropertyName("notify")]
        public bool Notify { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; } = SD.Status_Pending;
        [JsonPropertyName("totalSpentAmount")]
        public int DailySpentAmount { get; set; }//average of total spent amount till now and the days passed
        [JsonPropertyName("totalSavedAmount")]
        public int TotalSpentAmount { get; set;}//till now for the particular category and timing
        [JsonPropertyName("dailyRecommendedAmount")]
        public int DailyRecommendedAmount { get; set; } //to not exceed the daily budget
        [JsonPropertyName("estimatedDate")]
        public DateTime EstimatedDate { get; set; } //for the budget money to run out
        [JsonPropertyName("amountSpentThisWeek")]
        public int AmountSpentThisWeek { get; set; } //to track the progress
        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }
}
