using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GameOfLife.Common.Converters
{
    public class BooleanReConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (value as bool?) ?? throw new ArgumentException(nameof(value));
            return !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (value as bool?) ?? throw new ArgumentException(nameof(value));
            return !val;
        }
    }
}
