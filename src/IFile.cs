using System;

namespace restlessmedia.Module.File
{
  public interface IFile
  {
    int? FileId { get; set; }

    string FileName { get; set; }

    string SystemFileName { get; set; }

    string MimeType { get; set; }

    int? Flags { get; set; }

    DateTime? LastUpdated { get; set; }
  }
}