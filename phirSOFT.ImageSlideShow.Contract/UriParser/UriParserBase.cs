using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using phirSOFT.ImageSlideShow.Attributes;

namespace phirSOFT.ImageSlideShow.UriParser
{
    public abstract class UriParserBase<T> : IUriParser<T>
    {
        private static readonly Dictionary<Type, ImmutableHashSet<string>> _supportedSchemes = new Dictionary<Type, ImmutableHashSet<string>>();
        public UriParserBase()
        {
            var myType = GetType();
            if (_supportedSchemes.ContainsKey(myType))
                return;

            var supportedSchems = ImmutableHashSet.Create(myType.GetTypeInfo().GetCustomAttributes<SupportedSchemeAttribute>().Select(att => att.SupportedSchema).ToArray());
            _supportedSchemes.Add(myType, supportedSchems);
        }

        public bool CanParse(Uri uri)
        {
            return _supportedSchemes[GetType()].Contains(uri.Scheme) && CanParseInternal(uri);
        }

        protected abstract bool CanParseInternal(Uri uri);


        public abstract T Parse(Uri uri);

        object IUriParser.Parse(Uri uri)
        {
            return Parse(uri);
        }
    }
}
