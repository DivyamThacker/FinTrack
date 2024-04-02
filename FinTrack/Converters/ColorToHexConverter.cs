using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Converters
{
    public class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorName = value?.ToString()?.ToLower();
            string hexCode = string.Empty;

            switch (colorName)
            {
                case "red":
                    hexCode = "#FF0000";
                    break;
                case "green":
                    hexCode = "#00FF00";
                    break;
                //case "blue":
                //    hexCode = "#0000FF";
                //    break;
                // Add more cases for other colors if needed

                default:
                    throw new ArgumentException("Invalid color name");
            }

            return hexCode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
