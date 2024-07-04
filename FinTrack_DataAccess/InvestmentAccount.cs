using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class InvestmentAccount : Account
    {
        public InvestmentAccount()
        {
            Trades = new List<Trade>();
            Holdings = new List<Holding>();
        }

        public ICollection<Trade> Trades { get; set; }
        public ICollection<Holding> Holdings { get; set; }
    }

}
