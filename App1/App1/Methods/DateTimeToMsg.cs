using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Methods
{
    public class DateTimeToMsg : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                DateTime time = (DateTime)value;
                if(time == DateTime.MinValue)
                {
                    return "Kein Training bisher";
                } else
                {
                    return (time);
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
