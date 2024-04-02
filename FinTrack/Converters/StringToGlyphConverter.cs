using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Converters
{
    public class StringToGlyphConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string glyphString)
            {
                // Assuming your glyph code starts with "&#x":
                if (glyphString.StartsWith("&#x"))
                {
                    string hexCode = glyphString.Substring(3, 4); // Extract hex code (4 characters after "&#x")
                    int glyphCode = int.Parse(hexCode); // Parse as hex
                    return (char)glyphCode;
                }
                else
                {
                    // Handle other glyph string formats (if applicable)
                    return null; // Or provide a default value
                }
            }

            return null; // Return null for invalid input
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
