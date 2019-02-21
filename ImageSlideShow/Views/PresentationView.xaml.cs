using phirSOFT.ImageSlideShow.ViewModels;
using SecondaryViewsHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class PresentationView : Page
    {
        ViewLifetimeControl thisViewControl;
        CoreDispatcher mainDispatcher;
        int mainViewId;
        PresentationViewModel vm;

        public PresentationView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var initData = (ProjectionViewPageInitializationData)e.Parameter;

            // The ViewLifetimeControl is a convenient wrapper that ensures the
            // view is closed only when the user is done with it
            thisViewControl = initData.ProjectionViewPageControl;
            mainDispatcher = initData.MainDispatcher;
            mainViewId = initData.MainViewId;
            vm = initData.vm;

            // Listen for when it's time to close this view
            thisViewControl.Released += thisViewControl_Released;

            Focus(FocusState.Programmatic);
        }

        private async void thisViewControl_Released(object sender, EventArgs e)
        {
            // There are two major cases where this event will get invoked:
            // 1. The view goes unused for some time, and the system cleans it up
            // 2. The app calls "StopProjectingAsync"
            thisViewControl.Released -= thisViewControl_Released;
            await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                vm.Projection = null;
            });
            Window.Current.Close();

        }

        private async void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (vm.EndPresentation.CanExecute()) ;
                    vm.EndPresentation.Execute();
                });
            }
        }

        private async void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {

            await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (vm.EndPresentation.CanExecute())
                    vm.EndPresentation.Execute();
            });
        }

        private async void FirstSlide(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {

            await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (vm.First.CanExecute())
                    vm.First.Execute();
            });
        }

        private async void LastSlide(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {

            await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (vm.Last.CanExecute())
                    vm.Last.Execute();
            });
        }

        private async void PrevSlide(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {

            await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (vm.Prev.CanExecute())
                    vm.Prev.Execute();
            });
        }

        private async void NextSlide(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {

            await mainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (vm.Next.CanExecute())
                    vm.Next.Execute();
            });
        }
    }
}
