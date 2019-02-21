using CommonServiceLocator;
using phirSOFT.ImageSlideShow.Drawers;
using phirSOFT.ImageSlideShow.Drawers.XamlDrawers;
using phirSOFT.ImageSlideShow.ViewModels;
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

namespace phirSOFT.ImageSlideShow.Views
{
    public sealed partial class EditSlideView
    {
        public EditSlideView()
        {
            this.InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var ex = bg.GetBindingExpression(XamlDrawer.SourceProperty);
            bg.SetBinding(XamlDrawer.SourceProperty, new Binding() { Path = new PropertyPath("Background"), Source = DataContext});
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var ex = bg.GetBindingExpression(XamlDrawer.SourceProperty);
        }
    }
}
