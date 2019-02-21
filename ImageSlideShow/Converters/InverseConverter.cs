using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converters
{
    class InverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return parameter is IValueConverter Converter ? Converter.ConvertBack(value, targetType, null, language) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return parameter is IValueConverter Converter ? Converter.Convert(value, targetType, null, language) : null;
        }
    }
}
