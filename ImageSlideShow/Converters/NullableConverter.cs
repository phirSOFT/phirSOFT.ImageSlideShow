using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converters
{
    class NullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value ?? false;
        }
    }
}
