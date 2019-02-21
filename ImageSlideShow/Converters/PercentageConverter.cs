using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converters
{
    class PercentageConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var culture = new CultureInfo(language);
            return System.Convert.ToDouble(value, culture) * System.Convert.ToDouble(parameter ?? 1.0, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
