using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    public interface IDrawingCanvas : ICanvasResourceCreator, INotifyPropertyChanged
    {
        Size Size {get; }
        Point Center { get;}
    }
}
