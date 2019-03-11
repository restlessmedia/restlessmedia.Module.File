using restlessmedia.Module.Configuration;
using System.Configuration;

namespace restlessmedia.Module.File.Configuration
{
  public class FileSizeCollection : TypedCollection<FileSize>
  {
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((FileSize)element).Name;
    }

    public override void Remove(FileSize item)
    {
      if (BaseIndexOf(item) > 0)
      {
        BaseRemove(item.Name);
      }
    }
  }
}