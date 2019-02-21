using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;

#nullable enable
namespace phirSOFT.ImageSlideShow.Drawers
{
    class OverlayDrawer : QueueDrawer<ICanvasBrush?>
    {
        private CanvasGeometry? _shape;
        public OverlayDrawer(Dissolver dissolver) : base(dissolver)
        {
        }

        public Rect Bounds { get; set; }

        public float InnerRadius { get; set; }

        public float OuterRadius { get; set; }

        public CanvasGeometry Shape { get => _shape ?? throw new NotInitializedException(); set => Interlocked.Exchange(ref _shape, value); }


        protected override IGraphicsEffectSource GetSource(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, ICanvasBrush? item, TimeSpan created)
        {


            var list = new CanvasCommandList(sender);
            if (item != null)
                using (var session = list.CreateDrawingSession())
                {

                    session.FillGeometry(_shape, item);
                    session.DrawGeometry(_shape, Color.FromArgb(255, 63, 63, 63), 2);
                }
            return list;
        }

        public override Task CreateResourcesAsync(ICanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
