using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTrack_Models
{
    public class TransactionDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [Required]
        [JsonPropertyName("senderUsername")]
        public string SenderUsername { get; set; } = "me";
        [Required]
        [JsonPropertyName("recieverUsername")]
        public string RecieverUsername { get; set; } = "me";
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
        [JsonPropertyName("isUserSender")]
        public bool IsUserSender { get; set; } = true;
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("transactionDate")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }
}

