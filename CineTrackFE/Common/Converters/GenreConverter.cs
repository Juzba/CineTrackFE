using CineTrackFE.Models;
using System.Globalization;
using System.Windows.Data;

namespace CineTrackFE.Common.Converters
{
    class GenreConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> genres && genres.Count > 0)
            {
                return genres[0];  // Vrací první žánr ze seznamu
            }
            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Genre genre)
            {
                return new List<string> { genre.Name };
            }
            return new List<string>();
        }
    }
}
