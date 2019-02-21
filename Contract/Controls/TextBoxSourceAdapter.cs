using phirSOFT.ImageSlideShow.UriParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace phirSOFT.ImageSlideShow.Controls
{
    class TextBoxSourceAdapter : SourceEditorAdapterBase<TextBox>
    {
        public override ISourceEditor  Adapt(TextBox control)
        {
            return new Adapter(control);
        }

        private class Adapter : ISourceEditor
        {
            private readonly TextBox control;
            private static readonly Uri baseUri = new Uri("text:");
            private static readonly TextUriParser uriParser = new TextUriParser();

            public Adapter(TextBox control)
            {
                this.control = control;
                control.TextChanged += (sender, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
            }

            public Uri Source
            {
                get => new Uri(baseUri, control.Text);
                set
                {
                    if(uriParser.CanParse(value))
                    {
                        control.Text = uriParser.Parse(value);
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
