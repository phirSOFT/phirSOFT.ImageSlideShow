using System;
using System.Collections.Generic;
using System.Web;

namespace phirSOFT.ImageSlideShow.UriParsers
{
    public static class UriParserExtensions
    {
        public static bool TryParse(this IUriParser parser, Uri uri, Type targetType, out object result)
        {
            if (parser.CanParse(uri, targetType))
            {
                try
                {
                    result = parser.Parse(uri, targetType);
                    return true;
                }
                catch (ParseException)
                {

                }
            }
            result = default;
            return false;
        }

        public static T Parse<T>(this IUriParser parser, Uri uri)
        {
            return (T)parser.Parse(uri, typeof(T));
        }

        public static bool TryParse<T>(this IUriParser parser, Uri uri, out T result)
        {
            if (parser.TryParse(uri, typeof(T), out var directResult))
            {
                result = (T)directResult;
                return true;
            }
            result = default;
            return false;
        }

        public static IDictionary<string, string> ParseQuery(this string query)
        {
            var dict = new Dictionary<string, string>();
            var parseResult = HttpUtility.ParseQueryString(query);

            for (int i = 0; i < parseResult.Count; ++i)
            {
                dict.Add(parseResult.GetKey(i), parseResult.Get(i));
            }
            return dict;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue fallbackValue)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : fallbackValue;
        }

        public static TResult GetValue<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue, TResult> converter, TResult fallback)
        {
            return dictionary.ContainsKey(key) ? converter(dictionary[key]) : fallback;
        }

    }
}
