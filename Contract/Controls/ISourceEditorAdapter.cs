using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Controls
{
    interface ISourceEditorAdapter
    {
        ISourceEditor Adapt(object control);
    }

    interface ISourceEditorAdapter<in T> : ISourceEditorAdapter
    {
        ISourceEditor Adapt(T control);
    }
}
