using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class AccountDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("balance")]
        public decimal Balance { get; set; } = 0;

        [Required]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
