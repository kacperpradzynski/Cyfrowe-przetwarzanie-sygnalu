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
    public class VisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string param = (string)parameter;
            string arg = ((string)value).Substring(1, 2);
            switch (arg)
            {
                case "S1":
                    if (param == "F" || param == "R")
                        return Visibility.Visible;
                    break;
                case "Q1":
                    if (param == "N")
                        return Visibility.Visible;
                    break;
                case "Q2":
                    if (param == "N")
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
