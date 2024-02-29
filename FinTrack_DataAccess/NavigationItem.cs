using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace FinTrack_DataAccess
{
    public class NavigationItem
    {
        [Required]
        public string? Glyph { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public ICommand? NavigationBtnCommand { get; set; }
    }
}
