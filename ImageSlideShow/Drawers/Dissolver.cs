using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using phirSOFT.ImageSlideShow.Drawers.XamlDrawers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.Storage;
using Windows.UI;

namespace phirSOFT.ImageSlideShow.Drawers
{
    public class Dissolver
    {
        private readonly IDrawingCanvas _canvas;
        private PixelShaderEffect _dissolveEffect;
        public Dissolver(IDrawingCanvas canvas)
        {
            _canvas = canvas;
            canvas.PropertyChanged += canvasChanged;
        }

        private void canvasChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (new[] {nameof(IDrawingCanvas.Size), nameof(IDrawingCanvas.Center) }.Contains(e.PropertyName))
                UpdateMask();
        }

        public void ConfigureDissolve(float dissolveFactor, IGraphicsEffectSource source)
        {
            dissolveFactor = 0.5f * ((float)Math.Tanh((4f * dissolveFactor) - 2f) + 1);
            if (dissolveFactor > 1f)
                dissolveFactor = 1f;


            _dissolveEffect.Properties["dissolveAmount"] = 1 - dissolveFactor;
            _dissolveEffect.Source1 = source;
        }

        public void UpdateMask()
        {
            var start = _canvas.Size.ToVector2() * _canvas.Center.ToVector2();
            var end = new Vector2(start.X > _canvas.Size.Width - start.X ? 0.0f : (float)_canvas.Size.Width,
                start.Y > _canvas.Size.Height - start.Y ? 0.0f : (float)_canvas.Size.Height);

            var radius = (end - start).Length();

            var commandList = new CanvasCommandList(_canvas);

            using (var drawingSession = commandList.CreateDrawingSession())
            {
                using (var brush = new CanvasRadialGradientBrush(_canvas, Colors.White, Colors.Black)
                {
                    Center = start,
                    RadiusX = radius,
                    RadiusY = radius
                })
                    drawingSession.FillRectangle(new Rect(new Point(0, 0), _canvas.Size), brush);
            }


            _dissolveEffect.Source2 = new Transform2DEffect
            {
                Source = commandList
            };
        }

        internal async Task CreateResources()
        {
            _dissolveEffect = new PixelShaderEffect(await ReadAllBytes("Assets/Dissolve.bin"));
            UpdateMask();
        }

        public static async Task<byte[]> ReadAllBytes(string filename)
        {
            var uri = new Uri("ms-appx:///" + filename);
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var buffer = await FileIO.ReadBufferAsync(file);

            return buffer.ToArray();
        }

        public static implicit operator PixelShaderEffect(Dissolver dissolver)
        {
            return dissolver._dissolveEffect;
        }
    }
}
