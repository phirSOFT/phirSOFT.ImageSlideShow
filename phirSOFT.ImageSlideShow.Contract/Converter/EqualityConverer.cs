using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converter
{
    internal class EqualityConverer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is bool b && b ? parameter : null;
        }
    }
}
