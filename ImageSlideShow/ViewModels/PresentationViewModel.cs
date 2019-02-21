using CommonServiceLocator;
using phirSOFT.ImageSlideShow.Services;
using phirSOFT.ImageSlideShow.Views;
using Prism.Commands;
using Prism.Mvvm;
using SecondaryViewsHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace phirSOFT.ImageSlideShow.ViewModels
{
    class PresentationViewModel : MultiWindowViewModelBase
    {
        private SlideViewModel currentSlide;
        private int currentIndex;
        private ViewLifetimeControl projection;
        private CancellationTokenSource clickSource;

        public ObservableCollection<SlideViewModel> Slides { get; } = new ObservableCollection<SlideViewModel>();

        public SlideViewModel CurrentSlide
        {
            get => currentSlide;
            set => SetProperty(ref currentSlide, value, () =>
            {
                clickSource?.Cancel();
                if (Projection != null && CurrentSlide.TransitionAfter.HasValue)
                {
                    clickSource = new CancellationTokenSource();
                    Task.Run(async () =>
                    {
                        await Task.Delay(CurrentSlide.TransitionAfter.Value, clickSource.Token);
                        if (CurrentIndex < Slides.Count - 1)
                            CurrentIndex++;
                        else
                        {
                            EndPresentation.Execute();
                        }
                    });
                }
            });
        }

        public int CurrentIndex { get => currentIndex; set => SetProperty(ref currentIndex, value, () => CurrentSlide = Slides[CurrentIndex]); }

        public DelegateCommand CaptureSlide { get; }

        public DelegateCommand Next { get; }

        public DelegateCommand Prev { get; }

        public DelegateCommand First { get; }

        public DelegateCommand Last { get; }

        public DelegateCommand DeleteCurrent { get; }

        public DelegateCommand StartPresentation { get; }

        public DelegateCommand SwapScreens { get; }
        public DelegateCommand EndPresentation { get; }

        public DelegateCommand Save { get; }

        public DelegateCommand Open { get; }

        internal ViewLifetimeControl Projection { get => projection; set => SetProperty(ref projection, value); }

        public PresentationViewModel()
        {
            Slides.Add(ServiceLocator.Current.GetInstance<SlideViewModel>());
            Save = new DelegateCommand(async () =>
            {
                var picker = new FileSavePicker()
                {
                    DefaultFileExtension = ".issx"
                };
                picker.FileTypeChoices.Add("Bilderpräsentation", new List<string>() { ".issx" });
                var file = await picker.PickSaveFileAsync();

                if (file == null)
                    return;

                using (StorageStreamTransaction fs = await file.OpenTransactedWriteAsync())
                {
                    using (var archive = new ZipArchive(fs.Stream.AsStream(), ZipArchiveMode.Create))
                    {
                        var doc = new XDocument(
                            new XElement("Presentation",
                            Slides.Where(s => s.Recorded)
                                .Select(s =>
                                    new XElement((XName)"Slide",
                                        new XAttribute("Background", s.BackgroundImage.Source?.ToString() ?? string.Empty),
                                        new XAttribute("CornerImage", s.CornerImage.Source?.ToString() ?? string.Empty),
                                        new XAttribute("Title", s.Title.Source?.ToString() ?? string.Empty),
                                        new XAttribute("Image1", s.Image1.Source?.ToString() ?? string.Empty),
                                        new XAttribute("Image2", s.Image2.Source?.ToString() ?? string.Empty),
                                        new XAttribute("Image3", s.Image3.Source?.ToString() ?? string.Empty),
                                        new XAttribute("Transition", s.TransitionAfter?.ToString() ?? string.Empty)
                                    )
                                )
                                .ToArray()
                            )
                        );

                        using (var content = archive.CreateEntry(".content").Open())
                        {
                            doc.Save(content);
                        }

                        foreach (var item in ServiceLocator.Current.GetInstance<StorageAdapter>())
                        {
                            using (var resource = archive.CreateEntry(item.Key.ToString()).Open())
                            {
                                var stream = (await item.Value.GetStreamAsync()).AsStream();
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(resource);
                            }
                        }
                    }
                    await fs.CommitAsync();
                }

            });

            Open = new DelegateCommand(async () =>
            {
                var picker = new FileOpenPicker();

                picker.FileTypeFilter.Add(".issx");
                var file = await picker.PickSingleFileAsync();

                if (file == null)
                    return;

                var storage = ServiceLocator.Current.GetInstance<StorageAdapter>();
                storage.Dispose();

                var fs = (await file.OpenReadAsync()).AsStream();
                var slideShow = new ZipArchive(fs, ZipArchiveMode.Read);

                foreach (var item in slideShow.Entries)
                {
                    if (Guid.TryParse(item.Name, out var guid))
                    {
                        storage.RegisterStorageItem(guid, new ZipEntryStreamProvider(item));
                    }
                }
                Slides.Clear();
                var content = slideShow.GetEntry(".content");

                var doc = XDocument.Load(content.Open());

                foreach (var svm in doc.Root.Elements("Slide").Select(s =>
                 {
                     var sm = new SlideViewModel(storage)
                     {
                         TransitionAfter = TimeSpan.TryParse(s.Attribute("Transition").Value, out var span) ? span : (TimeSpan?)null,
                     };
                     sm.BackgroundImage.Source = !String.IsNullOrEmpty(s.Attribute("Background").Value) ? new Uri(s.Attribute("Background").Value) : null;
                     sm.Title.Source = !String.IsNullOrEmpty(s.Attribute("Title").Value) ? new Uri(s.Attribute("Title").Value) : null;
                     sm.CornerImage.Source = !String.IsNullOrEmpty(s.Attribute("CornerImage").Value) ? new Uri(s.Attribute("CornerImage").Value) : null;
                     sm.Image1.Source = !String.IsNullOrEmpty(s.Attribute("Image1").Value) ? new Uri(s.Attribute("Image1").Value) : null;
                     sm.Image2.Source = !String.IsNullOrEmpty(s.Attribute("Image2").Value) ? new Uri(s.Attribute("Image2").Value) : null;
                     sm.Image3.Source = !String.IsNullOrEmpty(s.Attribute("Image3").Value) ? new Uri(s.Attribute("Image3").Value) : null;
                     sm.Recorded = true;
                     return sm;
                 }))
                {
                    Slides.Add(svm);
                }

                CurrentIndex = 0;
                CurrentSlide = Slides[0];
            });

            CaptureSlide = new DelegateCommand(() =>
            {
                var newSlide = ServiceLocator.Current.GetInstance<SlideViewModel>();
                newSlide.BackgroundImage.Source = CurrentSlide.BackgroundImage.Source;
                newSlide.Title.Source = CurrentSlide.Title.Source;
                newSlide.CornerImage.Source = CurrentSlide.CornerImage.Source;
                newSlide.Image1.Source = CurrentSlide.Image1.Source;
                newSlide.Image2.Source = CurrentSlide.Image2.Source;
                newSlide.Image3.Source = CurrentSlide.Image3.Source;
                CurrentSlide.Recorded = true;
                Slides.Insert(CurrentIndex + 1, newSlide);
                CurrentIndex++;
            });

            Next = new DelegateCommand(() => CurrentIndex++, () => CurrentIndex < Slides.Count - 1).ObservesProperty(() => CurrentIndex).ObservesProperty(() => Slides.Count);
            Prev = new DelegateCommand(() => CurrentIndex--, () => CurrentIndex > 0).ObservesProperty(() => CurrentIndex);
            First = new DelegateCommand(() => CurrentIndex = 0);
            Last = new DelegateCommand(() => CurrentIndex = Slides.Count - 1);
            DeleteCurrent = new DelegateCommand(() =>
                    {
                        Slides.RemoveAt(CurrentIndex);
                        if (CurrentIndex > Slides.Count - 1)
                            CurrentIndex--;
                        else
                        {
                            CurrentSlide = Slides[CurrentIndex];
                        }

                    }, () => Slides.Count > 1).ObservesProperty(() => Slides.Count);

            StartPresentation = new DelegateCommand(async () =>
                    {
                        var dispatcher = Window.Current.Dispatcher;
                        var view = ApplicationView.GetForCurrentView().Id;
                        await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Projection = ViewLifetimeControl.CreateForCurrentView();

                    var initData = new ProjectionViewPageInitializationData();
                    initData.MainDispatcher = dispatcher;
                    initData.ProjectionViewPageControl = Projection;
                    initData.MainViewId = view;
                    initData.vm = this;

                    // Display the page in the view. Note that the view will not become visible
                    // until "StartProjectingAsync" is called
                    var rootFrame = new Frame();
                    rootFrame.Navigate(typeof(PresentationView), initData);
                    Window.Current.Content = rootFrame;

                    // The call to Window.Current.Activate is required starting in Windos 10.
                    // Without it, the view will never appear.
                    Window.Current.Activate();
                });
                        try
                        {
                            // Start/StopViewInUse are used to signal that the app is interacting with the
                            // view, so it shouldn't be closed yet, even if the user loses access to it
                            Projection.StartViewInUse();

                            // Show the view on a second display (if available) or on the primary display
                            await ProjectionManager.StartProjectingAsync(Projection.Id, view);

                            Projection.StopViewInUse();

                            if (Projection != null && CurrentSlide.TransitionAfter.HasValue)
                            {
                                clickSource = new CancellationTokenSource();
                                await Task.Run(async () =>
                        {
                            await Task.Delay(CurrentSlide.TransitionAfter.Value, clickSource.Token);
                            if (CurrentIndex < Slides.Count)
                                CurrentIndex++;
                            else
                            {
                                EndPresentation.Execute();
                            }
                        });
                            }

                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }, () => Projection == null).ObservesProperty(() => Projection);

            SwapScreens = new DelegateCommand(async () =>
                    {
                        var view = ApplicationView.GetForCurrentView().Id;
                        await Projection.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    // The view might arrive on the wrong display. The user can
                    // easily swap the display on which the view appears
                    Projection.StartViewInUse();
                    await ProjectionManager.SwapDisplaysForViewsAsync(Projection.Id, view);
                    Projection.StopViewInUse();
                });
                    }, () => Projection != null).ObservesProperty(() => Projection);


            EndPresentation = new DelegateCommand(async () =>
                    {
                        var view = ApplicationView.GetForCurrentView().Id;
                        await Projection.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Projection.StartViewInUse();
                    await ProjectionManager.StopProjectingAsync(Projection.Id, view);
                    Projection.StopViewInUse();
                });
                    }, () => Projection != null).ObservesProperty(() => Projection);






            CurrentIndex = 0;
            CurrentSlide = Slides[0];
        }
    }
}
