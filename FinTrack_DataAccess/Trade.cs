using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class Trade
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccountId { get; set; }
        public InvestmentAccount InvestmentAccount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Type { get; set; } // Buy or Sell
        [Required]
        public string Symbol { get; set; }
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; } = 0;
        public double MarketValue { get; set; }
        //public double HoldingValue { get; set; }
    }
}
