using restlessmedia.Module.Configuration;
using System.Configuration;

namespace restlessmedia.Module.File.Configuration
{
  public class DiskAccessCollection : TypedCollection<DiskAccess>
  {
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((DiskAccess)element).Access;
    }

    public override void Remove(DiskAccess item)
    {
      if (BaseIndexOf(item) > 0)
      {
        BaseRemove(item.Access);
      }
    }

    public DiskAccess this[AccessType access]
    {
      get { return (DiskAccess)BaseGet(access); }
    }

    public bool Exists(AccessType access)
    {
      return BaseGet(access) != null;
    }
  }
}