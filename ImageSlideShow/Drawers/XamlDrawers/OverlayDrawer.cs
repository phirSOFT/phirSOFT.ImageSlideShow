using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

#nullable enable
namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    class OverlayDrawer : ImageDrawerBase<ICanvasBrush?>
    {
        private Drawers.OverlayDrawer? _drawer;
        protected override QueueDrawer<ICanvasBrush?> CreateDrawerInternal(IDrawingCanvas resourceCreator, Dissolver dissolver)
        {
            _drawer = new Drawers.OverlayDrawer(dissolver);
            _drawer.Shape = CreateOverlayMask(resourceCreator);

            return _drawer;
        }



        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OuterRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OuterRadiusProperty =
            DependencyProperty.Register("OuterRadius", typeof(double), typeof(OverlayDrawer), new PropertyMetadata(0.0, OnRadiusChanged));

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OverlayDrawer)d).UpdateMask();
        }

        private void UpdateMask()
        {
            if (Canvas != null && _drawer != null)
                _drawer.Shape = CreateOverlayMask(Canvas);
        }

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OuterRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(OverlayDrawer), new PropertyMetadata(0.0));




        protected override async Task<ICanvasBrush?> TransformSource(Uri uri)
        {
            
            var image = await CreateImageAsync(uri);
            
            return image != null ? new CanvasImageBrush(Canvas, image)
            {
                Transform = Matrix3x2.CreateTranslation(Bounds.GetRectangle(Canvas).GetOffset()),
                SourceRectangle = Bounds.GetRectangle(Canvas)
            } : null;
        }

        private CanvasGeometry? CreateOverlayMask(IDrawingCanvas sender)
        {
            var _canvas = sender;
            if (_canvas == null)
                return null;
            return CanvasGeometry
                .CreateCircle(sender, _canvas.Center.ToVector2() * _canvas.Size.ToVector2(), (float)(OuterRadius * _canvas.Size.Height))
                .CombineWith(
                    CanvasGeometry.CreateRectangle(sender, Bounds.GetRectangle(_canvas)),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Intersect)
                .CombineWith(
                    CanvasGeometry.CreateCircle(sender, _canvas.Center.ToVector2() * _canvas.Size.ToVector2(), (float)(InnerRadius * _canvas.Size.Height)),
                    Matrix3x2.Identity,
                    CanvasGeometryCombine.Exclude
                );
        }

    }
}
