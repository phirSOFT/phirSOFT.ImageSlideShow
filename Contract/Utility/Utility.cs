using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace phirSOFT.ImageSlideShow.Utility
{
    internal static class Utility
    {
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
