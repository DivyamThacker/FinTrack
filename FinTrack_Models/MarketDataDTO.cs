using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class MarketDataDTO
    {
        [Required]
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; } //name of the stock
        [Required]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; } //current price of the stock 
        [JsonPropertyName("open")]
        public double Open { get; set; }
        [JsonPropertyName("close")]
        [Required]
        public double Close { get; set; }
        [JsonPropertyName("low")]
        public double Low { get; set; }
        [JsonPropertyName("high")]
        public double High { get; set; }
        [JsonPropertyName("volume")]
        public double Volume { get; set; }
        [JsonPropertyName("percentageChange")]
        public double PercentageChange { get; set; }
        //public string Exchange { get; set; }
        //public string Country { get; set; }
        //public string Currency { get; set; }
    }
}
