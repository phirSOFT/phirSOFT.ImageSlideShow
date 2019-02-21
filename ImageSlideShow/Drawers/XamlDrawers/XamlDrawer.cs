using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using phirSOFT.ImageSlideShow.Controls;
using phirSOFT.ImageSlideShow.Drawers.XamlDrawers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace phirSOFT.ImageSlideShow.Drawers
{
    public abstract class XamlDrawer : FrameworkElement
    {
        public abstract Task<IDrawer> CreateDrawerAsync(IDrawingCanvas canvas,  Dissolver dissolver);

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(XamlDrawer), new PropertyMetadata(new Uri("color:12345678"), OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { 
            var drawer = (XamlDrawer)d;
            drawer.UpdateSource((Uri) e.NewValue);
        }

        protected abstract void UpdateSource(Uri source);
    }
}
