using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FinTrack_Common;

namespace FinTrack_Models
{
    public class GoalDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("userId")]
        public int UserId { get; set; } = 1;

        [Required]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; } = DateTime.Now;
        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }  = DateTime.Now;
        [Required]
        [JsonPropertyName("period")]
        public string? Period { get; set; }
        [Required]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [JsonPropertyName("category")]
        public string? Category { get; set; } = SD.Category_All;
        [JsonPropertyName("notify")]
        public bool Notify { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; } = SD.Status_Pending;
        [JsonPropertyName("totalSavedAmount")]
        public int TotalSavedAmount { get; set; }//same as total Icome amount till now minus expenses (for now)
        [JsonPropertyName("dailySavedAmount")]
        public int DailySavedAmount { get; set; }//average of total saved amount till now and the days passed
        [JsonPropertyName("dailyRecommendedAmount")]
        public int DailyRecommendedAmount { get; set; } //to reach the daily goal
        [JsonPropertyName("estimatedDate")]
        public DateTime EstimatedDate { get; set; } //to reach the goal amount
        [JsonPropertyName("amountSavedThisWeek")]
        public int AmountSavedThisWeek { get; set; } //to track the progress
        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }
}
