using CommonServiceLocator;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using phirSOFT.ImageSlideShow.Converters;
using phirSOFT.ImageSlideShow.Model;
using phirSOFT.ImageSlideShow.Services;
using phirSOFT.ImageSlideShow.UriParsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#nullable enable
namespace phirSOFT.ImageSlideShow.Drawers.XamlDrawers
{
    abstract class ImageDrawerBase<T> : XamlDrawer
    {
        private QueueDrawer<T>? _drawer;
        private IDrawingCanvas? _canvas;
        private readonly ColorUriParser _colorUriParser = new ColorUriParser()
        {
            
        };
        private GaussianBlurEffect _blurEffect = new GaussianBlurEffect()
        {
            Optimization = EffectOptimization.Quality
        };
        private readonly ResourceProvider resouceProvicer;
        private readonly ImageResourceUriParser _imageResourceUriParser = new ImageResourceUriParser();
        public ImageDrawerBase()
        {
            resouceProvicer = ServiceLocator.Current.GetInstance<ResourceProvider>();
        }

        

        protected abstract Task<T> TransformSource(Uri uri);

        public IPositioning Bounds { get; set; } = new FillPositioning();
        protected IDrawingCanvas? Canvas { get => _canvas; private set => _canvas = value; }

        protected async Task<ICanvasImage?> CreateImageAsync(Uri? imageUri)
        {
            if (imageUri == null)
                return null;

            CanvasCommandList image = new CanvasCommandList(Canvas);

            var bounds = Bounds.GetRectangle(Canvas);

            switch (imageUri.Scheme)
            {
                case "color":
                    CreateMonochromeImage(imageUri, image, bounds);
                    break;
                case "image":
                    await CreateBitmapImage(imageUri, image, bounds);
                    break;
                case "text":
                    CreateTextImage(imageUri, image, bounds);
                    break;
            }
            return image;

        }

        private void CreateTextImage(Uri imageUri, CanvasCommandList image, Rect bounds)
        {
            using(var session = image.CreateDrawingSession())
            {
                session.Clear(Color.FromArgb(250, 255,255,255));
                bounds.X +=20;
                bounds.Width -=20;
                session.DrawText(HttpUtility.UrlDecode(imageUri.AbsolutePath, Encoding.Default), bounds, Color.FromArgb(255, 63, 63, 63), new CanvasTextFormat()
                {
                    HorizontalAlignment = CanvasHorizontalAlignment.Left,
                    VerticalAlignment = CanvasVerticalAlignment.Center,
                    FontSize = 70,
                });
            }
        }

        protected async Task CreateBitmapImage(Uri imageUri, CanvasCommandList image, Rect bounds)
        {
            var imageResouce = _imageResourceUriParser.Parse<ImageResource>(imageUri);
            CanvasBitmap bitmap = await ServiceLocator.Current.GetInstance<ResourceProvider>().GetCanvasBitmapAsync(imageResouce.Bitmap);
            Rect targetRect;
            Rect sourceRect = imageResouce.SourceRectangle ?? new Rect(new Point(), bitmap.Size);

            targetRect = StretchIntoRect(sourceRect.GetSize(), bounds, imageResouce.Strech);
            using (var drawingSession = image.CreateDrawingSession())
            {
                //drawingSession.Clear(Colors.Black);
                if (imageResouce.EnableBlur
                    && (imageResouce.Strech == Stretch.None || imageResouce.Strech == Stretch.Uniform)
                    && (sourceRect.Width < bounds.Width || sourceRect.Height < bounds.Height))
                {
                    _blurEffect.Source = bitmap;
                    drawingSession.DrawImage(_blurEffect, bounds, sourceRect);
                }

                drawingSession.DrawImage(bitmap, targetRect, sourceRect);

            }
        }

        protected static Rect StretchIntoRect(Size sourceSize, Rect destinationRect, Stretch stretch)
        {
            Rect targetRect;

            switch (stretch)
            {
                case Stretch.None:
                    var center = sourceSize.CenterWithinRect(destinationRect);

                    targetRect = new Rect(center, sourceSize);
                    break;
                case Stretch.Fill:
                    targetRect = destinationRect;
                    break;
                case Stretch.Uniform:
                case Stretch.UniformToFill:
                    var scale = sourceSize.GetAspectRatio();
                    var targetRatio = destinationRect.GetAspectRatio();
                    Size targetSize = ((scale > targetRatio) ^ (stretch == Stretch.UniformToFill))
                        ? new Size(destinationRect.GetSize().Width, destinationRect.GetSize().Width / scale)
                        : new Size(destinationRect.GetSize().Height * scale, destinationRect.GetSize().Height);

                    var offset = targetSize.CenterWithinRect(destinationRect);
                    targetRect = new Rect(offset, targetSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return targetRect;
        }

        protected void CreateMonochromeImage(Uri imageUri, CanvasCommandList image, Rect bounds)
        {
            var color = _colorUriParser.Parse<Color>(imageUri);

            using (var drawingSession = image.CreateDrawingSession())
            {
                using (var brush = new CanvasSolidColorBrush(Canvas, color))
                    drawingSession.FillRectangle(bounds, brush);
            }
        }


        public override async Task<IDrawer> CreateDrawerAsync(IDrawingCanvas resourceCreator, Dissolver dissolver)
        {
            _drawer = CreateDrawerInternal(resourceCreator, dissolver);
            Canvas = resourceCreator;
            if (Source != null)
                _drawer.AddItem(await TransformSource(Source));
            return _drawer;
        }

        protected override async void UpdateSource(Uri source)
        {
            _drawer?.AddItem(await TransformSource(source));
        }

        protected abstract QueueDrawer<T> CreateDrawerInternal(IDrawingCanvas resourceCreator, Dissolver dissolver);
    }
}
