using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.UI;

namespace phirSOFT.ImageSlideShow.Drawers
{
    class TextOverlayDrawer : OverlayDrawer
    {
        public TextOverlayDrawer(Dissolver dissolver) : base(dissolver)
        {
        }

        protected ICanvasBrush GetBackgroundBrush(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, string item, TimeSpan ellapsed)
        {
            var commands = new CanvasCommandList(sender);
            var b = TransformBounds(Bounds);
            using (var session = commands.CreateDrawingSession())
            {
                session.Clear(Color.FromArgb(255, 255, 255, 255));
                session.DrawLine((float)b.X, (float)b.Y, (float)b.X + (float)b.Width, (float)b.Y + (float)b.Height, Colors.Black);
                session.DrawLine((float)b.X, (float)b.Y + (float)b.Height, (float)b.X + (float)b.Width, (float)b.Y, Colors.Black);
                session.DrawText(item, TransformBounds(Bounds), Color.FromArgb(155, 63, 63, 63), new CanvasTextFormat()
                {
                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    FontSize = 30f
                });

               
            }

            var brush = new CanvasImageBrush(sender, commands);
            brush.SourceRectangle = b;
            brush.Transform = Matrix3x2.CreateTranslation((float) b.X, (float) b.Y);

 
            return brush;
        }

        private Rect TransformBounds(Rect b) => b;

    }

    class ImageOverlayDrawer : OverlayDrawer
    {
        public ImageOverlayDrawer(Dissolver dissolver) : base(dissolver)
        {
        }

        protected  ICanvasBrush GetBackgroundBrush(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, CanvasImageBrush item, TimeSpan ellapsed)
        {
            var b = Bounds; //TransformBounds(Bounds);
           

            var ib = item.SourceRectangle ?? item.Image.GetBounds(sender);

            var scale = b.Width / ib.Width;
            if (ib.Height * scale < b.Height)
            {
                scale = b.Height / ib.Height;
            }

            ib.Height *= scale;
            ib.Width *= scale;

            var offset = new Vector2((float)(b.Width - ib.Width), (float)(b.Height - ib.Height)) / 2f;

            
            var transform =  Matrix3x2.CreateScale((float) scale);// * Matrix3x2.CreateScale((float) (1f/scale));
            transform.Translation = new Vector2((float)b.X + offset.X, (float)b.Y + offset.Y);
            item.Transform = transform;
            return item;
        }
    }
}
