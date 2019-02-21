using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    class FillPositioning : IPositioning
    {
        public Rect GetRectangle(IDrawingCanvas canvas)
        {
           return new Rect(new Point(), canvas.Size);
        }
    }
}
