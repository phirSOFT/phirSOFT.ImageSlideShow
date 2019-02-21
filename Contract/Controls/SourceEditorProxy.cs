using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Controls
{
    class SourceEditorProxy : INotifyPropertyChanged
    {
        public Uri Source { get;set;}

        public IReadOnlyCollection<string> SupportedSchemas { get;}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
