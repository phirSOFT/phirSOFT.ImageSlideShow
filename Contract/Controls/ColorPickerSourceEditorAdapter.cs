using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace phirSOFT.ImageSlideShow.Controls
{
    public class ColorPickerSourceEditorAdapter : SourceEditorAdapterBase<ColorPicker>
    {
        public override ISourceEditor Adapt(ColorPicker control)
        {
            return new Adapter(control);
        }

        private class Adapter : ISourceEditor
        {
            private ColorPicker control;

            public Adapter(ColorPicker control)
            {
                this.control = control;
            }

            public Uri Source { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
