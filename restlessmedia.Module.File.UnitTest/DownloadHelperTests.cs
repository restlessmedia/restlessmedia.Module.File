using FakeItEasy;
using restlessmedia.Test;
using System;
using System.IO;
using Xunit;

namespace restlessmedia.Module.File.UnitTest
{
  public class DownloadHelperTests
  {
    public DownloadHelperTests()
    {
      _diskStorageProvider = A.Fake<IDiskStorageProvider>();
     _sampleUri = new Uri("http://somesite.com/image.jpg");
    }

    [Fact]
    public void TryDownload_adds_to_storage()
    {
      // set-up
      IDownloadable download = CreateDownload();
      string path = "foo/bar";
      string fileName = "foo.bar";
      DownloadHelper.WebClientFactory = () => A.Fake<IWebClient>();

      // call
      DownloadHelper.TryDownload(download, _diskStorageProvider, path, fileName);

      // assert
      A.CallTo(() => _diskStorageProvider.Put(path, fileName, A<Stream>.Ignored, A<string>.Ignored))
        .MustHaveHappenedOnceExactly();

      download.Status.MustBe(DownloadStatus.Success);
      download.Exception.MustBeNull();
    }

    [Fact]
    public void TryDownload_sets_exception()
    {
      // set-up
      IDownloadable download = CreateDownload();
      string path = "foo/bar";
      string fileName = "foo.bar";
      Exception exception = new Exception("foo");
      IWebClient webClient = A.Fake<IWebClient>();
      DownloadHelper.WebClientFactory = () => webClient;
      A.CallTo(() => webClient.OpenRead(_sampleUri))
        .Throws(exception);

      // call
      DownloadHelper.TryDownload(download, _diskStorageProvider, path, fileName).MustBeFalse();

      // assert
      download.Status.MustBe(DownloadStatus.Failed);
      download.Exception.MustBe(exception);
    }

    [Fact]
    public async void DownloadAsync_downloads()
    {
      // set-up
      IWebClient webClient = A.Fake<IWebClient>();
      DownloadHelper.WebClientFactory = () => webClient;
      IDownloadable download = CreateDownload();

      // call
      await DownloadHelper.DownloadAsync(download);

      // assert
      download.Status.MustBe(DownloadStatus.Success);
      download.Exception.MustBeNull();
    }

    [Fact]
    public async void DownloadAsync_sets_exception()
    {
      // set-up
      IDownloadable download = CreateDownload();
      Exception exception = new Exception("foo");
      IWebClient webClient = A.Fake<IWebClient>();
      DownloadHelper.WebClientFactory = () => webClient;
      A.CallTo(() => webClient.OpenReadTaskAsync(_sampleUri))
        .Throws(exception);

      // call
      try
      {
        await DownloadHelper.DownloadAsync(download);
      }
      catch { }

      // assert
      download.Status.MustBe(DownloadStatus.Failed);
      download.Exception.MustBe(exception);
    }

    private IDownloadable CreateDownload()
    {
      IDownloadable download = A.Fake<IDownloadable>();
      download.Exception = null;
      A.CallTo(() => download.Uri).Returns(_sampleUri);
      return download;
    }

    private readonly IDiskStorageProvider _diskStorageProvider;

    private readonly Uri _sampleUri;
  }
}
