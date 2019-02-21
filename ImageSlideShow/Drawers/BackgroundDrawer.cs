using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;

#nullable enable
namespace phirSOFT.ImageSlideShow.Drawers
{
    class BackgroundDrawer : QueueDrawer<ICanvasImage?>
    {

        public BackgroundDrawer(Dissolver dissolver) : base(dissolver)
        {

        }

        protected override IGraphicsEffectSource GetSource(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, ICanvasImage? item, TimeSpan created)
        {
            return item ?? new CanvasCommandList(sender);
        }

    }
}

