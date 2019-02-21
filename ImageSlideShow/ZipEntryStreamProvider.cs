using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using phirSOFT.ImageSlideShow.Services;
using Windows.Storage.Streams;

namespace phirSOFT.ImageSlideShow.ViewModels
{
    internal class ZipEntryStreamProvider : IStreamProvider
    {

        IRandomAccessStream stream;

        public ZipEntryStreamProvider(ZipArchiveEntry item)
        {
            var ms = new MemoryStream();
            item.Open().CopyTo(ms);
            stream = ms.AsRandomAccessStream();
        }

        public void Dispose()
        {
           
        }

        public Task<IRandomAccessStream> GetStreamAsync()
        {
            
            return Task.FromResult(stream);
        }
    }
}