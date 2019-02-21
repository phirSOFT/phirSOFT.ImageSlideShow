using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.Graphics.Effects;

namespace phirSOFT.ImageSlideShow.Drawers
{
    abstract class QueueDrawer<T> : IDrawer
    {
        private readonly LinkedList<(T, TimeSpan)> _items = new LinkedList<(T, TimeSpan)>();
        public Vector2 Center { get; set; }

        public Size Size
        {
            get => size;
            set
            {
                size = value;
                SizeChanged();
            }
        }
        public TimeSpan TransitionTime { get; set; }

        private TimeSpan _clock;
        private Size size;
        private readonly Dissolver dissolver;

        public QueueDrawer(Dissolver dissolver)
        {
            this.dissolver = dissolver;
        }

        protected virtual void SizeChanged() { }

        public void AddItem(T item)
        {
            _items.AddLast((item, _clock));
        }

        public void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var head = _items.First;

            while (head != null)
            {
                var ellapsedTime = (args.Timing.TotalTime - head.Value.Item2);
                if (ellapsedTime > TransitionTime && head.Previous != null)
                {
                    _items.RemoveFirst();
                }

                dissolver.ConfigureDissolve(ellapsedTime.Ticks / (float)TransitionTime.Ticks, GetSource(sender, args, head.Value.Item1, ellapsedTime));
                args.DrawingSession.DrawImage((PixelShaderEffect)dissolver, new Rect(new Point(0, 0), sender.Size), new Rect(new Point(0, 0), sender.Size));

                head = head.Next;
            }
        }

        protected abstract IGraphicsEffectSource GetSource(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, T item, TimeSpan created);

        public virtual Task CreateResourcesAsync(ICanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            return Task.CompletedTask;
        }

        public virtual void Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            _clock = args.Timing.TotalTime;
        }
    }
}
