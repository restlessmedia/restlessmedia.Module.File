using restlessmedia.Module.Data;

namespace restlessmedia.Module.File.Data
{
  public class FileDataProvider : FileSqlDataProvider, IFileDataProvider
  {
    public FileDataProvider(IDataContext context)
      : base(context) { }
  }
}