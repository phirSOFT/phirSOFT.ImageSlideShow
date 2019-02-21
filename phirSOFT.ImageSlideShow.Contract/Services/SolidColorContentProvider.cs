using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using phirSOFT.ImageSlideShow.Attributes;
using phirSOFT.ImageSlideShow.UriParser;
using Windows.Foundation;
using Windows.UI;

namespace phirSOFT.ImageSlideShow.Services
{
    [SupportedScheme("color")]
    public class SolidColorContentProvider : IContentProvider
    {
        private readonly IUriParser<Color> _parser;

        public SolidColorContentProvider(IUriParser<Color> parser)
        {
           _parser = parser;
        }
        public ICanvasImage CreateContent(ICanvasResourceCreator resourceCreator, Uri uri, Rect targetBounds)
        {
            
            var commandList = new CanvasCommandList(resourceCreator);
            try
            {
                var color = _parser.Parse(uri);
                using (var dc = commandList.CreateDrawingSession())
                {
                    dc.Clear(color);
                }
            }
            catch (ParseException)
            {
                // We catch this and reti
            }
           

            return commandList;            
        }
    }
}
