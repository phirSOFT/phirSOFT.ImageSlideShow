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
        public string Title { get; set;}

        public Control EditorView { get; set;}

        public ISourceEditor Editor { get; set;}

        public int SortIndex { get;}
    }
}
