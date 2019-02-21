using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace phirSOFT.ImageSlideShow.Converters
{
    class BoolArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(!(value is bool b))
                throw new NotSupportedException();

            return b ? TrueValue : FalseValue;
        }

        public object TrueValue { get;set;}

        public object FalseValue { get; set; }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
