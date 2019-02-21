using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.UriParsers
{
    public interface IUriParser
    {
        bool CanParse(Uri uri, Type targetType);

        object Parse(Uri uri, Type targetType);
    }

    public interface IUriParser<T> : IUriParser
    {
        bool CanParse(Uri uri);

        T Parse(Uri urI;)
    }
}
