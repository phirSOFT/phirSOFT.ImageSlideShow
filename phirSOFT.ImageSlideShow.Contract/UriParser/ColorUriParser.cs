using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace phirSOFT.ImageSlideShow.UriParser
{
    class ColorUriParser : UriParserBase<Color>
    {
        public override Color Parse(Uri uri)
        {
            var path = uri.AbsolutePath;

            try
            {
                if(path.Length < 8)
                {
                    path = "FF000000".Substring(0, 8 - path.Length) + path; 
                }

                byte a = byte.Parse(path.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte r = byte.Parse(path.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(path.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(path.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
            catch (FormatException ex)
            {
                throw new ParseException($"Could not parse '{path}' as a color.", ex);
            }
        }

        protected override bool CanParseInternal(Uri uri)
        {
            return true;
        }
    }
}
