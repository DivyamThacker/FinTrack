using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack
{
    //public enum Period
    //{
    //    Week,
    //    Month,
    //    Year, 
    //    OneTime
    //}

    //public enum Status
    //{
    //    Pending,
    //    Busted, 
    //    UnderSpent,
    //    OverSpent
    //}

    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required]
        public string? Period { get; set; }
        public int SpentAmount { get; set; }
        [Required]
        public int BudgetAmount { get; set; }
        [Required]
        public string? Status {  get; set; }
    }
}
