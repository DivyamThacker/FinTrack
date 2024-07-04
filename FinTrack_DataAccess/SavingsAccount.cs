using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_DataAccess
{
    public class SavingsAccount : Account
    {
        public SavingsAccount()
        {
            Transactions = new List<Transaction>();
            Records = new List<Record>();
            Budgets = new List<Budget>();
            Goals = new List<Goal>();
        }

        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Record> Records { get; set; }
        public ICollection<Budget> Budgets { get; set; }
        public ICollection<Goal> Goals { get; set; }
    }
}
