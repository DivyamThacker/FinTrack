using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class TradeDTO
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
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } // Buy or Sell
        [Required]
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } // name of stock
        [Required]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 0;
        [JsonPropertyName("marketValue")]
        public double MarketValue { get; set; }
        [JsonPropertyName("holdingValue")]
        public double HoldingValue { get; set; }
        [JsonPropertyName("isGain")]
        public bool IsGain { get; set; }
        [JsonPropertyName("percentageChange")]
        public double PercentageChange { get; set; }
        [JsonPropertyName("totalChange")]
        public double Totalchange { get; set;} //Total gain or loss
    }
}
