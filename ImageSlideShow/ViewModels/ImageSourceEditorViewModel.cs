using phirSOFT.ImageSlideShow.Services;
using phirSOFT.ImageSlideShow.UriParsers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;

namespace phirSOFT.ImageSlideShow.ViewModels
{
    class ImageSourceEditorViewModel : MultiWindowViewModelBase
    {
        private bool? blur;
        private EnumViewModel<Stretch> currentStretch;
        private Uri source;
        private readonly StorageAdapter storageAdapter;
        private static ImageResourceUriParser uriPArser = new ImageResourceUriParser();
        private bool hidden = true;
        private bool colorMode;
        private bool textMode;
        private bool imageMode;
        private bool recorded;   

        public Uri Source
        {
            get => source; set
            {
                SetProperty(ref source, value, () =>
                {
                    Hidden = ColorMode = TextMode = ImageMode = false;
                    if (source == null)
                    {
                        Hidden = true;
                    }
                    else if (source.Scheme == "text")
                        TextMode = true;
                    else if (Source.Scheme == "color")
                        ColorMode = true;
                    else if (Source.Scheme == "image")
                    {
                        ImageMode = true;
                        var r = uriPArser.Parse<Model.ImageResource>(value);
                        Blur = r.EnableBlur;
                        CurrentStretch = new EnumViewModel<Stretch>(r.Strech);
                    }

                }
                );

            }
        }

        public bool Hidden
        {
            get => hidden;
            set => SetProperty(ref hidden, value, () =>
            {
                if (Hidden)
                    Source = null;
            });
        }

        public Uri Text
        {
            get => Source;
            set
            {
                if (TextMode)
                    Source = value;
            }
        }

        public Uri ColorSource
        {
            get => Source;
            set
            {
                if (ColorMode)
                    Source = value;
            }
        }

        public IReadOnlyList<EnumViewModel<Stretch>> Stretches { get; } = new List<EnumViewModel<Stretch>>(((Stretch[])Enum.GetValues(typeof(Stretch))).Select(e => new EnumViewModel<Stretch>(e)));

        public EnumViewModel<Stretch> CurrentStretch { get => currentStretch; set => SetProperty(ref currentStretch, value, UpdateUri); }

        public DelegateCommand SelectImage { get; }

        public bool ColorMode { get => colorMode; set => SetProperty(ref colorMode, value); }

        public bool TextMode { get => textMode; set => SetProperty(ref textMode, value); }

        public bool ImageMode { get => imageMode; set => SetProperty(ref imageMode, value); }

        public ImageSourceEditorViewModel(StorageAdapter storageAdapter)
        {
            SelectImage = new DelegateCommand(async () =>
            {
                var file = await SelectFileAsync();
                if (file != null)
                    Source = file;
            });
            this.storageAdapter = storageAdapter;
        }


        public bool? Blur { get => blur; set => SetProperty(ref blur, value, UpdateUri); }

        async Task<Uri> SelectFileAsync()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
                return new Uri($"image:{storageAdapter.RegisterFile(file).ToString()}?stretch={CurrentStretch.Name}&blur={Blur ?? false}");
            return null;
        }

        void UpdateUri()
        {
            if (Source != null && Source.Scheme != "color")
            {
                Source = new Uri($"{Source.Scheme}:{Source.AbsolutePath}?stretch={CurrentStretch.Name}&blur={Blur ?? false}");
            }
        }

        internal struct EnumViewModel<T> : IEquatable<EnumViewModel<T>> where T : Enum
        {
            public EnumViewModel(T value)
            {
                Value = value;
            }
            public T Value { get; }

            public string Name => Enum.GetName(typeof(T), Value);

            public bool Equals(EnumViewModel<T> other)
            {
                return other.Value.Equals(Value);
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }


}
