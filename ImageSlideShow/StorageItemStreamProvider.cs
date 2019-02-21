using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace phirSOFT.ImageSlideShow.Services
{
    internal class StorageItemStreamProvider : IStreamProvider
    {
        private StorageFile file;

        public StorageItemStreamProvider(StorageFile file)
        {
            this.file = file;
        }

        public void Dispose()
        {
            
        }

        public async Task<IRandomAccessStream> GetStreamAsync()
        {
            return await file.OpenReadAsync().AsTask();
        }
    }
}