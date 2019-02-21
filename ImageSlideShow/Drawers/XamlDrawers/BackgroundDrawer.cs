using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    class BackgroundDrawer : ImageDrawerBase<ICanvasImage?>
    {
        protected override QueueDrawer<ICanvasImage?> CreateDrawerInternal(IDrawingCanvas resourceCreator, Dissolver dissolver)
        {
            return new Drawers.BackgroundDrawer(dissolver);
        }

        protected override Task<ICanvasImage?> TransformSource(Uri uri)
        {
            return CreateImageAsync(uri);
        }
    }
}
