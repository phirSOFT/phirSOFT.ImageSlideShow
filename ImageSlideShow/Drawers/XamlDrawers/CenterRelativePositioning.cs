using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    class CenterRelativePositioning : IPositioning
    {

        public Rect Rect { get; set;}
        public Rect GetRectangle(IDrawingCanvas canvas)
        {
            return new Rect(
                Rect.X * canvas.Size.Height + canvas.Center.X * canvas.Size.Width,
                Rect.Y * canvas.Size.Height + canvas.Center.Y * canvas.Size.Height,
                Rect.Width * canvas.Size.Height,
                Rect.Height * canvas.Size.Height);

        }
    }
}
