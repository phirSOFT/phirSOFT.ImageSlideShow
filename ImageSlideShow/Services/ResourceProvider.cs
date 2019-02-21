using Microsoft.Graphics.Canvas;
using Nito.AsyncEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace phirSOFT.ImageSlideShow.Services
{
    class ResourceProvider : IDisposable
    {
        private Dictionary<Guid, CanvasBitmap> _bitmaps = new Dictionary<Guid, CanvasBitmap>();
        private ICanvasResourceCreator _resourceCreator;
        private AsyncManualResetEvent _resourceCreated = new AsyncManualResetEvent(false);

        public ResourceProvider(StorageAdapter adapter)
        {
            Adapter = adapter;
        }

        public StorageAdapter Adapter { get; }

        public void Initialize(ICanvasResourceCreator resourceCreator)
        {
            _resourceCreator = resourceCreator;
            _resourceCreated.Set();
        }

        private async Task LoadResource(Guid identifier)
        {
            await _resourceCreated.WaitAsync();
            if (!_bitmaps.ContainsKey(identifier))
                _bitmaps.Add(identifier, await CanvasBitmap.LoadAsync(_resourceCreator, await Adapter.GetStorageItem(identifier)));
        }

        public async Task<CanvasBitmap> GetCanvasBitmapAsync(Guid identifier)
        {
            if (!_bitmaps.ContainsKey(identifier))
                await LoadResource(identifier);

            return _bitmaps[identifier];
        }

        public void Dispose()
        {
            foreach (var bitmap in _bitmaps.Values)
            {
                bitmap?.Dispose();
            }
        }
    }

    class StorageAdapter : IDisposable, IEnumerable<KeyValuePair<Guid, IStreamProvider>>
    {
        private Dictionary<Guid, IStreamProvider> _streams = new Dictionary<Guid, IStreamProvider>();

        public void Clear()
        {
            Dispose();
            _streams.Clear();
        }


        public Guid RegisterFile(StorageFile file)
        {
            var guid = Guid.NewGuid();
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
            RegisterStorageItem(guid, new StorageItemStreamProvider(file));
            return guid;
        }

        public void RegisterStorageItem(Guid guid, IStreamProvider item)
        {
            if (_streams.TryGetValue(guid, out var oldItem))
            {
                oldItem?.Dispose();
                _streams[guid] = item;
            }
            else
            {
                _streams.Add(guid, item);
            }
        }

        public async Task<IRandomAccessStream> GetStorageItem(Guid identifier)
        {
            return await _streams[identifier].GetStreamAsync();
        }

        public void Dispose()
        {
            foreach (var item in _streams.Values)
            {
                item?.Dispose();
            }
        }

        public IEnumerator<KeyValuePair<Guid, IStreamProvider>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Guid, IStreamProvider>>)_streams).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Guid, IStreamProvider>>)_streams).GetEnumerator();
        }
    }

    interface IStreamProvider : IDisposable
    {
        Task<IRandomAccessStream> GetStreamAsync();
    }
}
