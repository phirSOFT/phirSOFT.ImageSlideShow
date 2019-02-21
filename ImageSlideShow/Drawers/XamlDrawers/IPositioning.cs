using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    interface IPositioning
    {
        Rect GetRectangle(IDrawingCanvas canvas);
    }
}
