using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Converters
{
    class ListInverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            // Check if value is an IEnumerable (including List<T>)
            if (value is MarshalingObservableCollection collection)
            {
                // Cast collection to a generic list (if possible)
                var list = collection as List<object>;

                // If it's a List<object>, directly use List.Reverse()
                if (list != null)
                {
                    list.Reverse();
                    return list;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            // If none of the above conditions are met, return null
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

  }
