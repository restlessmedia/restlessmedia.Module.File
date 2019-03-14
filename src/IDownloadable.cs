using System;

namespace restlessmedia.Module.File
{
  public interface IDownloadable
  {
    Uri Uri { get; }

    DownloadStatus Status { get; set; }

    Exception Exception { get; set; }
  }
}