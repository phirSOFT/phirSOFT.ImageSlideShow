using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.UriParser
{
    public static class UriParserExtensions
    {
        public static bool TryParse(this IUriParser parser, Uri uri, out object result)
        {
            if (parser.CanParse(uri))
            {
                try
                {
                    result = parser.Parse(uri);
                    return true;
                }
                catch
                {

                }
            }
            result = default;
            return false;
        }

        public static T Parse<T>(this IUriParser parser, Uri uri)
        {
            return (T)parser.Parse(uri);
        }

        public static bool TryParse<T>(this IUriParser parser, Uri uri, out T result)
        {
            if (parser.TryParse(uri, out var directResult) && directResult is T castedResult)
            {
                result = castedResult;
                return true;
            }
            result = default;
            return false;
        }
    }
}
