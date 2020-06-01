using restlessmedia.Module.File;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace restlessmedia.Business.Helper
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

      download.Status = DownloadStatus.Success;

      using (DownloadClient client = new DownloadClient())
      {
        try
        {
          storage.Put(path, fileName, client.OpenRead(download.Uri));
        }
        catch (Exception e)
        {
          download.Status = DownloadStatus.Failed;
          download.Exception = e;
          return false;
        }
      }

      return true;
    }

    public static Stream Download(IDownloadable download)
    {
      if (download == null || download.Uri == null)
      {
        throw new ArgumentNullException(nameof(download));
      }

      download.Status = DownloadStatus.Success;

      using (DownloadClient client = new DownloadClient())
      {
        try
        {
          return client.OpenRead(download.Uri);
        }
        catch (Exception e)
        {
          download.Status = DownloadStatus.Failed;
          download.Exception = e;
          throw;
        }
      }
    }

    public static Task<Stream> DownloadAsync(IDownloadable download)
    {
      if (download == null || download.Uri == null)
      {
        throw new ArgumentNullException(nameof(download));
      }

      using (DownloadClient client = new DownloadClient())
      {
        return client.OpenReadTaskAsync(download.Uri);
      }
    }

    public static void Download(IDownloadable download, IDiskStorageProvider storage, string path, string fileName)
    {
      if (storage == null)
      {
        throw new ArgumentNullException(nameof(storage));
      }

      download.Status = DownloadStatus.Success;

      try
      {
        storage.Put(path, fileName, Download(download));
      }
      catch (Exception e)
      {
        download.Status = DownloadStatus.Failed;
        download.Exception = e;
      }
    }

    public static Task DownloadAsync(IDownloadable download, IDiskStorageProvider storage, string path, string fileName)
    {
      if (storage == null)
      {
        throw new ArgumentNullException(nameof(storage));
      }

      return DownloadAsync(download).ContinueWith(x =>
      {
        if (x.IsFaulted)
        {
          download.Status = DownloadStatus.Failed;
          download.Exception = x.Exception;
        }
        else if (x.IsCompleted)
        {
          download.Status = DownloadStatus.Success;
          storage.Put(path, fileName, x.Result);
        }
      });
    }

    private class DownloadClient : WebClient
    {
      public DownloadClient(int connectionLimit = 15)
      {
        _connectionLimit = connectionLimit;
      }

      protected override WebRequest GetWebRequest(Uri address)
      {
        HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
        request.ServicePoint.ConnectionLimit = _connectionLimit;
        return request;
      }

      private readonly int _connectionLimit;
    }
  }
}