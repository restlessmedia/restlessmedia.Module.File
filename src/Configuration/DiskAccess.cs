using System;
using System.Configuration;

namespace restlessmedia.Module.File.Configuration
{
  public class DiskAccess : ConfigurationElement, IDiskAccess
  {
    [ConfigurationProperty(accessProperty, IsRequired = true)]
    public AccessType Access
    {
      get
      {
        return (AccessType)this[accessProperty];
      }
    }

    [ConfigurationProperty(pathProperty, IsRequired = true)]
    public string Path
    {
      get
      {
        return (string)this[pathProperty];
      }
    }

    [ConfigurationProperty(baseUriProperty, IsRequired = false)]
    public Uri BaseUri
    {
      get
      {
        return (Uri)this[baseUriProperty];
      }
    }

    private const string accessProperty = "access";

    private const string pathProperty = "path";

    private const string baseUriProperty = "baseUri";
  }
}
