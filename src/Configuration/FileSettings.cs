using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace restlessmedia.Module.File.Configuration
{
  public class FileSettings : SerializableConfigurationSection, IFileSettings
  {
    [ConfigurationProperty(_supportedExtensionsProperty, IsRequired = false)]
    public string SupportedExtensions
    {
      get
      {
        return (string)this[_supportedExtensionsProperty];
      }
    }

    public IFileSize GetSize(string name)
    {
      return SizeCollection.Exists(name) ? SizeCollection[name] : null;
    }

    public IDiskAccess GetAccess(AccessType type)
    {
      return AccessCollection.Exists(type) ? AccessCollection[type] : null;
    }

    [ConfigurationProperty(_cdnProperty, IsRequired = false)]
    public string CDN
    {
      get
      {
        return (string)this[_cdnProperty];
      }
    }

    /// <summary>
    /// Internal copy of file sizes.  We don't expose this to the app because of the dependency on System.Configuration
    /// </summary>
    [ConfigurationProperty(_sizesProperty, IsRequired = false)]
    [ConfigurationCollection(typeof(FileSizeCollection), AddItemName = "add")]
    private FileSizeCollection SizeCollection
    {
      get { return (FileSizeCollection)this[_sizesProperty]; }
    }

    [ConfigurationProperty(_accessProperty, IsRequired = false)]
    [ConfigurationCollection(typeof(DiskAccessCollection), AddItemName = "add")]
    private DiskAccessCollection AccessCollection
    {
      get { return (DiskAccessCollection)this[_accessProperty]; }
    }

    private const string _fileRootProperty = "fileRoot";

    private const string _logDirectoryProperty = "logDirectory";

    private const string _supportedExtensionsProperty = "supportedExtensions";

    private const string _sizesProperty = "sizes";

    private const string _accessProperty = "access";

    private const string _cdnProperty = "cdn";
  }
}