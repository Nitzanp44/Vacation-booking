using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using BE;
using System.Windows.Data;
using BL;

namespace PLWPF
{
    class MoneyConverter : IValueConverter
    {
        IBL bl = FactoryBL.GetBl();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int money = 0;
             Order ordi = (Order)value;
            try
            {
                if (ordi.Status == Enums.StatusOrder.נסגר_בהיענות_של_הלקוח)
                {
                    TimeSpan t = bl.getBLGuestRequest(ordi.numGuestRequest).ReleaseDate - bl.getBLGuestRequest(ordi.numGuestRequest).EntryDate;
                    money = t.Days * bl.configurationData("fee");
                }
                return money;
            }
            catch
            {
                return 0;
                throw new NotImplementedException("בעיית המרה");


            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
