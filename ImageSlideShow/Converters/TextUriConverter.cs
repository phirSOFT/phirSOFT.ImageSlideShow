using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.UriParsers
{
    class TextUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is Uri uri && uri.Scheme == "text" ? HttpUtility.UrlDecode(uri.AbsolutePath, Encoding.Default) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return new Uri($"text:{value}");
        }
    }
}
