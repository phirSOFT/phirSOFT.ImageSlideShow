using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.ImageSlideShow.Controls
{
    public interface ISourceEditor : INotifyPropertyChanged
    {
        Uri Source {get;set; }
    }
}
