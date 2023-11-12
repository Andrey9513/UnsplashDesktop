using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace UnsplashDesktop.UI.Converters
{
    public class StringToTimeoutConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is string))
            {
                throw new ArgumentException(nameof(StringToTimeoutConverter));
            }

            var timeoutStr = value.ToString();

            try
            {
                var timeoutValue = System.Convert.ToInt32(timeoutStr.Split(' ')[0]);
                var secInMin = 60;
                var secInHour = secInMin * 60;
                var secInDay = secInHour * 24;
                var secInWeek = secInDay * 7;

                var unit = timeoutStr.Split(' ')[1];
                var multiplier = unit switch
                {
                    "second" => 1,
                    "minute" => secInMin,
                    "hour" => secInHour,
                    "day" => secInDay,
                    "week" => secInWeek,
                    _ => 1
                };
                return timeoutValue * multiplier;
            }
            catch (Exception exc)
            {
                Log.Error(exc, exc.Message);
                return 10;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                throw new ArgumentException(nameof(StringToTimeoutConverter));
            }

            var timeoutValue=(int)value;

            var secInMin = 60;
            var secInHour = secInMin * 60;
            var secInDay = secInHour * 24;
            var secInWeek = secInDay * 7;
            if ((timeoutValue >= secInMin) && (timeoutValue < secInHour)) return $"{timeoutValue / secInMin} minute";
            if ((timeoutValue >= secInHour) && (timeoutValue < secInDay)) return $"{timeoutValue / secInHour} minute";
            if ((timeoutValue >= secInDay) && (timeoutValue < secInWeek)) return $"{timeoutValue / secInDay} minute";
            if ((timeoutValue >= secInWeek)) return $"{timeoutValue / secInWeek} minute";

            return $"{value} second";
        }
    }
}
