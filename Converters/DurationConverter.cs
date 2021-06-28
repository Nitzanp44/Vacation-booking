using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using BE;
using System.Windows.Data;

namespace Converters
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GuestRequest guestRequest = (GuestRequest)value;
            TimeSpan t = guestRequest.ReleaseDate - guestRequest.EntryDate;
            int duration = t.Days;

            return duration;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
