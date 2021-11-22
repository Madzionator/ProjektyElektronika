using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjektyElektronika.Client.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = System.Convert.ToBoolean(value);
            var param = parameter is null || System.Convert.ToBoolean(parameter);

            return boolValue == param
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
