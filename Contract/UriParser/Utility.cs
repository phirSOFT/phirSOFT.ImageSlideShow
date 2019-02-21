using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow.UriParser
{
    public static class Utility
    {
        public static bool TryParse(string rectString, out Rect rect)
        {
            try
            {
                var segments = rectString.Split(';');

                rect = new Rect(
                    double.Parse(segments[0]),
                    double.Parse(segments[1]),
                    double.Parse(segments[2]),
                    double.Parse(segments[3])
                );
                return true;
            }
            catch (Exception e) when (e is FormatException || e is IndexOutOfRangeException) 
            {
                rect = default;
                return false;
            }
        }

        public static T TryParse<T>(string s, Func<string, T> parser, T defaultValue = default)
        {
            try
            {
                return parser(s) ?? defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
            
        }
    }
}
