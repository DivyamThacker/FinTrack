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
        public string UserId { get; set; }
        [Required]
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } // Foreign key to Account model
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
        //[JsonPropertyName("ThisWeekAmount")]
        //public int ThisWeekAmount { get; set; }
        //[JsonPropertyName("ThisMonthAmount")]
        //public int ThisMonthAmount { get; set; }
        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }
}
