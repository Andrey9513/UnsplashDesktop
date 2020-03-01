using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using static System.Resources.ResXFileRef;

namespace UnsplashDesktop.UI.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                throw new ArgumentException(nameof(StringToTimeoutConverter));
            }
            var res = value.ToString();
            return res;
        }
    }
}
