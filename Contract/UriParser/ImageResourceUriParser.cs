using phirSOFT.ImageSlideShow.Attributes;
using phirSOFT.ImageSlideShow.Model;
using phirSOFT.ImageSlideShow.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using static phirSOFT.ImageSlideShow.UriParser.Utility;

namespace phirSOFT.ImageSlideShow.UriParser
{
    [SupportedScheme("color")]
    class ImageResourceUriParser : UriParserBase<ImageResource>
    {
        protected override bool CanParseInternal(Uri uri)
        {
            // Can we perform a simple check and not parse the uri twice?
            return true;
        }

        public override ImageResource Parse(Uri uri)
        {
            var query = uri.Query.ParseQuery();
            var res = new ImageResource()
            {
                Bitmap = Guid.Parse(uri.AbsolutePath),
                Strech = query.GetValue("stretch", s => TryParse(s, stretch => Enum.Parse<Stretch>(stretch)), Stretch.None),
                EnableBlur = query.GetValue("blur", blur => blur == bool.TrueString, false),
                SourceRectangle = query.GetValue("bounds", bounds => TryParse(bounds, out var b) ? b : (Rect?)null, null)
            };

            return res;
        }
    }
}