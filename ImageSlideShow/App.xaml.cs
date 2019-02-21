using Prism.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.Registration;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Prism.Ioc;
using phirSOFT.ImageSlideShow.Services;
using Prism;
using phirSOFT.ImageSlideShow.Views;
using phirSOFT.ImageSlideShow.ViewModels;
using Prism.Navigation;
using CommonServiceLocator;
using Unity;
using Unity.Lifetime;

namespace phirSOFT.ImageSlideShow
{
    /// <summary>
    /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterForNavigation<EditSlideView, PresentationViewModel>(nameof(Views.EditSlideView));
            container.RegisterSingleton<StorageAdapter>();
            container.GetContainer().RegisterType<ResourceProvider>(new PerThreadLifetimeManager());
            container.RegisterSingleton<IServiceLocator, UnityServiceLocatorAdapter>();
            ServiceLocator.SetLocatorProvider(() => container.GetContainer().Resolve<IServiceLocator>());
        }

       
        protected override void OnStart(StartArgs args)
        {
            NavigationService.NavigateAsync(nameof(Views.EditSlideView));
        }
    }
}
