using phirSOFT.ImageSlideShow.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace phirSOFT.ImageSlideShow.Services
{
    class SourceEditorContext
    {
        public string Title { get;}

        public Control EditorView { get;}

        public ISourceEditor Editor { get;}

        public int SortIndex { get;}
    }
}
