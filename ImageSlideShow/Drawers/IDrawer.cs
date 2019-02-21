using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Drawers
{
    public interface IDrawer
    {
        [Obsolete]
        Vector2 Center { get;set;}
        TimeSpan TransitionTime { get; set; }


        Windows.Foundation.Size Size { get;set;}

        void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args);

        Task CreateResourcesAsync(ICanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args);

        void Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args);
    }
}
