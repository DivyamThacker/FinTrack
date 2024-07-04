using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class HoldingDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [Required]
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        [Required]
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [Required]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("marketValue")]
        public double MarketValue { get; set; }
        [JsonPropertyName("isGain")]
        public bool IsGain { get; set; }
        [JsonPropertyName("percentageChange")]
        public double PercentageChange { get; set; }
        [JsonPropertyName("totalChange")]
        public double TotalChange { get; set; } //total difference between market value and holding value * quantity (gain or loss)
    }
}
