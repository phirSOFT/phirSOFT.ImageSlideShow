using phirSOFT.ImageSlideShow.UriParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converters
{
    class UriConverter : IValueConverter
    {
        private ColorUriParser _parser = new ColorUriParser();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           return value is Uri uri && _parser.TryParse<Color>(uri, out var color) ? color : Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Color color)
                return new Uri($"color:{color.A:x2}{color.R:x2}{color.G:x2}{color.B:x2}");
            return null;
        }
    }
}
