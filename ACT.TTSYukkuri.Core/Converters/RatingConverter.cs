using System;
using System.Globalization;
using System.Windows.Data;

namespace ACT.TTSYukkuri.Converters
{
    public class RatingConverter :
        IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) => System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
