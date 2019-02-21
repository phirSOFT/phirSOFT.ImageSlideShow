using phirSOFT.ImageSlideShow.Model;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace phirSOFT.ImageSlideShow.UriParsers
{
    class ImageResourceUriParser : UriParserBase<ImageResource>
    {
      
        public ImageResourceUriParser() : base(new[] { "image" })
        {
        }

        protected override ImageResource Parse(Uri uri)
        {
            var query = uri.Query.ParseQuery();

            bool TryParseBounds(string bounds, out Rect b)
            {

                try
                {
                    var segments = bounds.Split(';');

                    b = new Rect(
                        double.Parse(segments[0]),
                        double.Parse(segments[1]),
                        double.Parse(segments[2]),
                        double.Parse(segments[3])
                    );
                    return true;
                }
                catch (Exception)
                {
                    b = default;
                    return false;
                }
            }

            var res = new ImageResource()
            {
                Bitmap = Guid.Parse(uri.AbsolutePath),
                Strech = query.GetValue("stretch", stretch => Enum.TryParse<Stretch>(stretch, out var s) ? s : Stretch.None, Stretch.None),
                EnableBlur = query.GetValue("blur", blur => blur == bool.TrueString, false),
                SourceRectangle = query.GetValue("bounds", bounds => TryParseBounds(bounds, out var b) ? b : (Rect?)null, null)
            };

            return res;
        }
    }

}
