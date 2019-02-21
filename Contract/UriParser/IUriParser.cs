using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.UriParser
{
    public interface IUriParser
    {
        bool CanParse(Uri uri);

        object Parse(Uri uri);
    }

    public interface IUriParser<out T> : IUriParser
    {
        new T Parse(Uri uri);
    }
}
