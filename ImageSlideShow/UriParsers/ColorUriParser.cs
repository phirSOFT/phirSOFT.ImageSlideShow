using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace phirSOFT.ImageSlideShow.UriParsers
{
    class ColorUriParser : UriParserBase<Color>
    {
        public ColorUriParser() : base(new [] {"color"})
        {
        }

        protected override Color Parse(Uri uri)
        {
            var path = uri.AbsolutePath;

            try
            {
                int offset = path.StartsWith("#") ? 1 : 0;

                byte a = byte.Parse(path.Substring(0 + offset, 2), System.Globalization.NumberStyles.HexNumber);
                byte r = byte.Parse(path.Substring(2 + offset, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(path.Substring(4 + offset, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(path.Substring(6 + offset, 2), System.Globalization.NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            catch(FormatException ex)
            {
                throw new ParseException($"Could not parse '{path}' as a color.");
            }
        }
    }

}
