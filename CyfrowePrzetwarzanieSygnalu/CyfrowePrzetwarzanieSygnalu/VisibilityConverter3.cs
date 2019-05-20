using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CyfrowePrzetwarzanieSygnalu
{
    public class VisibilityConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string param = (string)parameter;
            string arg = ((string)value).Substring(1, 2);
            switch (arg)
            {
                case "O1":
                    if (param == "S2")
                        return Visibility.Visible;
                    break;
                case "O2":
                    if (param == "F" || param == "W" || param == "M" || param == "K")
                        return Visibility.Visible;
                    break;
                case "O3":
                    if (param == "N" || param == "S2")
                        return Visibility.Visible;
                    break;
                case "O4":
                    if (param == "t" || param == "v")
                        return Visibility.Visible;
                    break;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
