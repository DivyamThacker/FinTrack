using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class MarketData
    {
        public string Symbol { get; set; }
        public string Name { get; set; } //name of the stock
        public DateTime Date { get; set; }
        public double Price { get; set; } //current price of the stock 
        public double Open { get; set; }
        public double Close { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Volume { get; set; }
        //public string Exchange { get; set; }
        //public string Country { get; set; }
        //public string Currency { get; set; }
    }
}
