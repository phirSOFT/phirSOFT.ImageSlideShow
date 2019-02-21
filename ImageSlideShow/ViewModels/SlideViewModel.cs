using phirSOFT.ImageSlideShow.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace phirSOFT.ImageSlideShow.ViewModels
{
    class SlideViewModel : MultiWindowViewModelBase
    {
        private bool thumbnailOutdates, recorded;
        private TimeSpan? transitionAfter;

        public ImageSourceEditorViewModel BackgroundImage { get; }
        public ImageSourceEditorViewModel CornerImage { get; }

        public ImageSourceEditorViewModel Title { get; }

        public ImageSourceEditorViewModel Image1 { get; }
        public ImageSourceEditorViewModel Image2 { get; }
        public ImageSourceEditorViewModel Image3 { get; }

        public bool Recorded { get => recorded; set => SetProperty(ref recorded, value); }

        public byte[] Thumbnail { get; set; } = null;

        public bool ThumbnailOutdates { get => thumbnailOutdates; set => SetProperty(ref thumbnailOutdates, value); }

        public TimeSpan? TransitionAfter
        {
            get => transitionAfter;
            set => SetProperty(ref transitionAfter, value, () =>
            {
                RaisePropertyChanged(nameof(Transition));
                RaisePropertyChanged(nameof(TransitionTime));
            });
        }

        public TimeSpan TransitionTime { get => TransitionAfter ?? TimeSpan.FromSeconds(2); set => TransitionAfter = value; }

        public bool Transition { get => TransitionAfter.HasValue; set => TransitionAfter = (value ? TransitionTime : (TimeSpan?)null); }
        public SlideViewModel(StorageAdapter storageAdapter)
        {


            BackgroundImage = new ImageSourceEditorViewModel(storageAdapter);
            CornerImage = new ImageSourceEditorViewModel(storageAdapter);
            Title = new ImageSourceEditorViewModel(storageAdapter);
            Image1 = new ImageSourceEditorViewModel(storageAdapter);
            Image2 = new ImageSourceEditorViewModel(storageAdapter);
            Image3 = new ImageSourceEditorViewModel(storageAdapter);
            StorageAdapter = storageAdapter;

        }

        private void Subscribe(INotifyPropertyChanged p)
        {
            p.PropertyChanged += UpdateThumbnail;
        }

        private async void UpdateThumbnail(object sender, PropertyChangedEventArgs e)
        {
            await Task.Delay(2000);
            ThumbnailOutdates = true;
        }

        public StorageAdapter StorageAdapter { get; }
    }
}
