using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CineTrackFE.Common.Converters;

public class VisibilityInverterConvertor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is Visibility visibility)
        {
            if (visibility == Visibility.Visible) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
