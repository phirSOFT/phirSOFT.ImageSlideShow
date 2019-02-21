using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converters
{
    class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((TimeSpan)value).ToString(@"hh\:mm\:ss\.fff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.ParseExact((string)value, @"hh\:mm\:ss\.fff", CultureInfo.CurrentUICulture);
        }
    }
}
