using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace phirSOFT.ImageSlideShow.UriParsers
{
    abstract class UriParserBase<T> : IUriParser
    {
        private readonly IEnumerable<string> supportedSchemas;

        protected UriParserBase(IEnumerable<string> supportedSchemas)
        {
            this.supportedSchemas = supportedSchemas;
        }
        public bool CanParse(Uri uri, Type targetType)
        {
            return targetType.IsAssignableFrom(typeof(T)) && supportedSchemas.Contains(uri.Scheme);
        }

        public object? Parse(Uri uri, Type targetType)
        {
            return Parse(uri);
        }

        protected abstract T Parse(Uri uri);
    }
}
