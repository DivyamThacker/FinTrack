using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class RecordDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userId")]
        [Required]
        public int UserId { get; set; } = 1;
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = "default";
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("recordDate")]
        public DateTime RecordDate { get; set; } = DateTime.Now;
        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 0;
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        [JsonPropertyName("isIncome")]
        public bool IsIncome { get; set; }
        //[JsonPropertyName("lastWeekAmount")]
        //public int LastWeekAmount { get; set; }
        //[JsonPropertyName("lastMonthAmount")]
        //public int LastMonthAmount { get; set; }
        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }
}
