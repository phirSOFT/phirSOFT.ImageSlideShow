using phirSOFT.ImageSlideShow.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace phirSOFT.ImageSlideShow.UriParser
{
    [SupportedScheme("text")]
    class TextUriParser : UriParserBase<string>
    {
        public TextUriParser() 
        {
        }

        public override string Parse(Uri uri)
        {
            return HttpUtility.HtmlDecode(uri.AbsolutePath);
        }

        protected override bool CanParseInternal(Uri uri)
        {
            return true;
        }
    }
}
