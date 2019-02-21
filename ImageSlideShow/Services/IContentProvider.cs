using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow.Services
{
    public interface IContentProvider
    {
         ICanvasImage CreateContent(ICanvasResourceCreator resourceCreator, Uri uri, Rect targetBounds);
    }
}
