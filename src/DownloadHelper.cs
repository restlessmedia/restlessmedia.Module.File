using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace restlessmedia.Module.File
{
  public static class DownloadHelper
  {
    public static bool TryDownload(IDownloadable download, IDiskStorageProvider storage, string path, string fileName)
    {
      if (download == null || download.Uri == null)
      {
        throw new ArgumentNullException(nameof(download));
      }

      if (storage == null)
      {
        throw new ArgumentNullException(nameof(storage));
      }

      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentNullException(nameof(path));
      }

      if (string.IsNullOrEmpty(fileName))
      {
        return false;
      }

      try
      {
        using (IWebClient client = CreateWebClient())
        {
          storage.Put(path, fileName, client.OpenRead(download.Uri));
        }
        download.Status = DownloadStatus.Success;
      }
      catch (Exception e)
      {
        download.Status = DownloadStatus.Failed;
        download.Exception = e;
        return false;
      }

      return true;
    }

    public static Task<Stream> DownloadAsync(IDownloadable download)
    {
      if (download == null || download.Uri == null)
      {
        throw new ArgumentNullException(nameof(download));
      }

      try
      {
        using (IWebClient client = CreateWebClient())
        {
          download.Status = DownloadStatus.Success;
          return client.OpenReadTaskAsync(download.Uri);
        }
      }
      catch (Exception e)
      {
        download.Status = DownloadStatus.Failed;
        download.Exception = e;
        throw e;
      }
    }

    public static async void DownloadAsync(IDownloadable download, IDiskStorageProvider storage, string path, string fileName)
    {
      if (download == null || download.Uri == null)
      {
        throw new ArgumentNullException(nameof(download));
      }

      if (storage == null)
      {
        throw new ArgumentNullException(nameof(storage));
      }

      try
      {
        Stream stream = await DownloadAsync(download);
        storage.Put(path, fileName, stream);
      }
      catch (Exception e)
      {
        download.Status = DownloadStatus.Failed;
        download.Exception = e;
        throw e;
      }
    }

    private static IWebClient CreateWebClient()
    {
      ServicePointManager.DefaultConnectionLimit = 15;
      return WebClientFactory();
    }

    internal static Func<IWebClient> WebClientFactory = () => new SystemNetWebClient();
  }
}