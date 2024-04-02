using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Entities
{
    public class RecordEntity
    {
        public string Month { get; set; }

        public double Target { get; set; }

        public RecordEntity(string xValue, double yValue)
        {
            Month = xValue;
            Target = yValue;
        }
    }
}
