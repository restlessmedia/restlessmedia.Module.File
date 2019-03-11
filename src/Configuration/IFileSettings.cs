namespace restlessmedia.Module.File.Configuration
{
  public interface IFileSettings
  {
    string SupportedExtensions { get; }

    IFileSize GetSize(string name);

    IDiskAccess GetAccess(AccessType type);

    string CDN { get; }
  }
}