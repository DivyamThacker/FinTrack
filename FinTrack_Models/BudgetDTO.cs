using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace FinTrack_Models
{
    public class BudgetDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter name..")]
        public required string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Required]
        public string? Period { get; set; }
        [Required]
        public int BudgetAmount { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}
