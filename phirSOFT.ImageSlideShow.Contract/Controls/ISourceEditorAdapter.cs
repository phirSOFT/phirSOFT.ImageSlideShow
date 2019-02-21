using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Controls
{
    interface ISourceEditorAdapter<T>
    {
        ISourceEditor Adapt(T control);
    }
}
