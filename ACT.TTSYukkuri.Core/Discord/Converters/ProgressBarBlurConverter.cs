using System;
using System.Globalization;
using System.Windows.Data;

namespace ACT.TTSYukkuri.Discord.Converters
{
    public class ProgressBarBlurConverter :
        IValueConverter
    {
        private const double ProgressBarBlurDefault = 10;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            if ((bool)value)
            {
                return ProgressBarBlurDefault;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
