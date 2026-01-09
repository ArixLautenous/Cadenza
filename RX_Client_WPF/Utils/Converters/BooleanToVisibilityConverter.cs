using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RX_Client.Utils.Converters
{
    // Class này phải là public để XAML nhìn thấy được
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // Chuyển từ ViewModel (bool) -> View (Visibility)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // True -> Hiện (Visible)
                // False -> Ẩn hoàn toàn (Collapsed - không chiếm chỗ)
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            // Mặc định là ẩn nếu giá trị null hoặc sai kiểu
            return Visibility.Collapsed;
        }

        // Chuyển ngược từ View -> ViewModel (Ít dùng, nhưng cần implement cho đủ interface)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
