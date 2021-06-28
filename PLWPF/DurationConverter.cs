using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using BE;
using System.Windows.Data;

namespace PLWPF
{
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int duration = 0;
            try
            {
                GuestRequest guestRequest = (GuestRequest)value;
                TimeSpan t = guestRequest.ReleaseDate - guestRequest.EntryDate;
                duration = t.Days;

                return duration;
            }
            catch
            {
                return 0;
                throw new NotImplementedException( "בעיית המרה");
                
                
            }
            
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
