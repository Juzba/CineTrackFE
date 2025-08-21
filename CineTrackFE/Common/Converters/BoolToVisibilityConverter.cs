using System.Globalization;
using System.Windows.Data;

namespace CineTrackFE.Common.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public bool IsInverted { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            if(IsInverted) boolValue = !boolValue;
            return boolValue ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        return System.Windows.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
