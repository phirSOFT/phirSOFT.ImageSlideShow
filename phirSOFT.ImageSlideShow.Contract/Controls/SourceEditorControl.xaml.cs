using phirSOFT.ImageSlideShow.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace phirSOFT.ImageSlideShow.Controls
{
    public sealed partial class SourceEditorControl : UserControl
    {
        private readonly SourceEditorProxy _editors;
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(SourceEditorControl), new PropertyMetadata(new Uri("null:"), SourceChanged));

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SourceEditorControl) d).Source = (Uri) e.NewValue;
        }

        public IReadOnlyCollection<string> SupportedSchemes
        {
            get { return (IReadOnlyCollection<string>)GetValue(SupportedSchemesProperty); }
            set { SetValue(SupportedSchemesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SupportedSchemes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SupportedSchemesProperty =
            DependencyProperty.Register("SupportedSchemes", typeof(IReadOnlyCollection<string>), typeof(SourceEditorControl), new PropertyMetadata(null));
        
     

        public SourceEditorControl()
        {
            this.InitializeComponent();
        }

    }
}
