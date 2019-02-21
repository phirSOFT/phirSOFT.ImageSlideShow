using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace phirSOFT.ImageSlideShow.Model
{
    public struct ImageResource
    {
        public Guid Bitmap { get; set; }
        public Rect? SourceRectangle { get; set; }

        public bool EnableBlur { get; set; }

        public Stretch Strech { get; set; }
    }
}
