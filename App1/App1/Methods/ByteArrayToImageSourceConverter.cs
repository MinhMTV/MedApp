using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;
using System.IO;

namespace App1.Methods
{
    class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource xValue = null;
            try
            {
                if (value != null)
                {

                    xValue = ImageSource.FromStream(
                        new Func<Stream>(() =>
                        {
                            MemoryStream mem = new MemoryStream((byte[])value);
                            return mem;
                        }));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return xValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
