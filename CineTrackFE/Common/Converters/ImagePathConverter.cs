using System.Configuration;
using System.Globalization;
using System.Windows.Data;

namespace CineTrackFE.Common.Converters
{
    public class ImagePathConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
        
            if (value is string ImageFileName && !string.IsNullOrEmpty(ImageFileName))
            {
                return $"pack://application:,,,/CineTrackFE;component/Common/Images/{ImageFileName}";
            }   
            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
