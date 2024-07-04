using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class Holding
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccountId { get; set; }
        public InvestmentAccount InvestmentAccount { get; set; }
        [Required]
        public string Symbol { get; set; }
        public string Name { get; set; } //name of the stock
        [Required]
        public int Quantity { get; set; } = 0;
        public double Price { get; set; } //average cost of the stock that you bought
        //public double MarketValue { get; set; }
    }
}
