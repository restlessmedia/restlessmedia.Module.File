using System;

namespace restlessmedia.Module.File
{
  public interface IDiskAccess
  {
    AccessType Access { get; }

    string Path { get; }

    Uri BaseUri { get; }
  }
}