using SecondaryViewsHelpers;
using Windows.UI.Core;

namespace phirSOFT.ImageSlideShow.ViewModels
{
    internal class ProjectionViewPageInitializationData
    {
        public CoreDispatcher MainDispatcher;
        public ViewLifetimeControl ProjectionViewPageControl;
        public int MainViewId;
        public PresentationViewModel vm;
    }
}