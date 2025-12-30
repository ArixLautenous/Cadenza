using System;
using System.Globalization;
using System.Windows.Data;

// Namespace này PHẢI KHỚP với dòng xmlns:converters trong App.xaml
namespace RX_Client_WPF.Utils.Converters
{
    // Class phải là public
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}