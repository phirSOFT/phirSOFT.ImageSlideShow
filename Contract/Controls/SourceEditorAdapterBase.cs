using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Controls
{
    public abstract class SourceEditorAdapterBase<T> : ISourceEditorAdapter<T>
    {
        public abstract ISourceEditor Adapt(T control);

        public ISourceEditor Adapt(object control)
        {
            return Adapt((T) control);
        }
    }
}
