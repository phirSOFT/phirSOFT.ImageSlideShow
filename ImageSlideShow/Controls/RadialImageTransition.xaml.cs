using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.UI;
using Windows.Storage;
using System.Diagnostics;
using Windows.Storage.Streams;
using phirSOFT.ImageSlideShow.Drawers;
using phirSOFT.ImageSlideShow.Converters;
using CommonServiceLocator;
using phirSOFT.ImageSlideShow.Services;
using Windows.UI.Xaml.Markup;
using phirSOFT.ImageSlideShow.Drawers.XamlDrawers;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Reflection;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace phirSOFT.ImageSlideShow.Controls
{
    [ContentProperty(Name = nameof(Drawers))]
    public sealed partial class RadialImageTransition : UserControl, IDrawingCanvas
    {
        private List<IDrawer> _drawers;

        public IList<XamlDrawer> Drawers { get; } = new List<XamlDrawer>();
        public RadialImageTransition()
        {
            this.InitializeComponent();
            _drawers = new List<IDrawer>();
        }



        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterXProperty =
            DependencyProperty.Register("CenterX", typeof(double), typeof(RadialImageTransition), new PropertyMetadata(0.0, CenterChanged));

        private static void CenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trantition = (RadialImageTransition)d;
            trantition._center = new Point(trantition.CenterX, trantition.CenterY);
            trantition._canvasChanged?.Invoke(d, new PropertyChangedEventArgs(nameof(IDrawingCanvas.Center)));
        }

        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }



        public byte[] Thumbnail
        {
            get { return (byte[])GetValue(ThumbnailProperty); }
            set { SetValue(ThumbnailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumbnail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbnailProperty =
            DependencyProperty.Register("Thumbnail", typeof(byte[]), typeof(RadialImageTransition), new PropertyMetadata(null));



        public bool ThumbnailOutdated
        {
            get { return (bool)GetValue(ThumbnailOutdatedProperty); }
            set { SetValue(ThumbnailOutdatedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbnailOutdated.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbnailOutdatedProperty =
            DependencyProperty.Register("ThumbnailOutdated", typeof(bool), typeof(RadialImageTransition), new PropertyMetadata(false, OnUpdateThumbnail));

        private static async void OnUpdateThumbnail(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var r = (RadialImageTransition)d;
            if ((bool)e.NewValue)
                await r.Canvas.RunOnGameLoopThreadAsync(async () =>
                {
                    var timing = new CanvasTimingInformation
                    {
                        ElapsedTime = TimeSpan.MaxValue,
                    };


                    var renderTarget = new CanvasRenderTarget(r.Canvas, (float)r.Canvas.Width, (float)r.Canvas.Height, 96);


                    r.Thumbnail = await CaptureThumbnailFromControl(r.Canvas, r.Canvas.Size, FindControlAndDrawMethod(), ds => timing);
                    r.ThumbnailOutdated = false;
                });

        }

        static MethodInfo FindControlAndDrawMethod()
        {
            // Look for a control of the specified type.

            // Look for the method that handles the Draw event. This is identified by having two parameters,
            // the first being the control and the second matching the type of its draw event args.
            var drawMethods = from method in typeof(CanvasAnimatedControl).GetRuntimeMethods()
                              where method.GetParameters().Length == 2
                              where method.GetParameters()[0].ParameterType == typeof(CanvasAnimatedControl)
                              where method.GetParameters()[1].ParameterType == typeof(CanvasAnimatedDrawEventArgs)
                              select method;

            return drawMethods.FirstOrDefault();
        }

        private static object GetDescendantsOfType<TControl>(UserControl parent) where TControl : class
        {
            throw new NotImplementedException();
        }

        static async Task<byte[]> CaptureThumbnailFromControl(ICanvasResourceCreator canvasControl, Size controlSize, MethodInfo drawMethod, Func<CanvasDrawingSession, object> createDrawEventArgs)
        {
            // Use reflection to invoke the same method the example would normally use to handle the Draw
            // event. Because we are calling this directly rather than the control raising the event in
            // the normal way, we can cunningly redirect the drawing into a rendertarget of our choosing.
            var renderTarget = new CanvasRenderTarget(canvasControl, (float)controlSize.Width, (float)controlSize.Height, 96);

            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.Transparent);

                object[] args = { canvasControl, createDrawEventArgs(ds) };

                drawMethod.Invoke(canvasControl, args);
            }

            using (var ms = new MemoryStream())
            {
                await renderTarget.SaveAsync(ms.AsRandomAccessStream(), CanvasBitmapFileFormat.Png);
                return ms.ToArray();
            }

        }


        // Using a DependencyProperty as the backing store for CenterY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterYProperty =
            DependencyProperty.Register("CenterY", typeof(double), typeof(RadialImageTransition), new PropertyMetadata(0.0, CenterChanged));



        public TimeSpan TransitionTime { get; set; } = TimeSpan.FromSeconds(1);

        private void CanvasAnimatedControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Clear(Colors.Black);
            foreach (var drawer in _drawers)
            {
                drawer.Draw(sender, args);
            }
        }

        private void CanvasAnimatedControl_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender, args).AsAsyncAction());
        }

        private async Task CreateResourcesAsync(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            ServiceLocator.Current.GetInstance<ResourceProvider>().Initialize(Canvas);
            var dissolver = new Dissolver(this);
            await dissolver.CreateResources();

            foreach (var xamlDrawer in Drawers)
            {
                var drawer = await xamlDrawer.CreateDrawerAsync(this, dissolver);
                drawer.TransitionTime = TransitionTime;
                await drawer.CreateResourcesAsync(sender, args);
                _drawers.Add(drawer);
            }

        }

        private void CanvasAnimatedControl_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            
            foreach (var drawer in _drawers)
            {
                drawer.Update(sender, args);
            }
            
        }



        Size IDrawingCanvas.Size => Canvas.Size;

        private Point _center;
        Point IDrawingCanvas.Center => _center;

        CanvasDevice ICanvasResourceCreator.Device => Canvas.Device;

        event PropertyChangedEventHandler _canvasChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                _canvasChanged += value;
            }

            remove
            {
                _canvasChanged -= value;
            }
        }
    }
}
