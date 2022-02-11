using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CBMTraining.Methods
{
    public class IntToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return ((int)value).ToString();
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = 0;
            int.TryParse(value.ToString(), out number);
            return number;
        }
    }
}