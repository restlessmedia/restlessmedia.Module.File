using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using restlessmedia.Module.File.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace restlessmedia.Module.File
{
  public class AzureStorageProvider : FileSystemStorageProvider
  {
    public AzureStorageProvider(IAzureSettings azureSettings, IFileSettings fileSettings)
      : base(fileSettings)
    {
      if (azureSettings == null || string.IsNullOrEmpty(azureSettings.Storage.ConnectionString))
      {
        throw new ConfigurationErrorsException("Azure configuration is missing or incorrect.  Check the azure config section exists.");
      }

      _account = CloudStorageAccount.Parse(azureSettings.Storage.ConnectionString);
    }

    public override byte[] Get(string path, object name)
    {
      using (Stream stream = Stream(path, name))
      {
        if (stream != null)
        {
          return stream.ReadAllBytes();
        }
      }

      return null;
    }

    public override T Get<T>(string path, object name)
    {
      using (Stream stream = Stream(path, name))
      {
        if (stream != null)
        {
          return Deserialize<T>(stream);
        }
      }

      return default(T);
    }

    public override Stream Stream(string path, object name)
    {
      CloudBlockBlob blob = GetBlob(path, name);

      if (!blob.Exists())
      {
        return null;
      }

      return blob.OpenRead(); 
    }

    public override void Put<T>(string path, object name, T obj, string contentType = null)
    {
      Put(path, name, Serialize(obj), contentType);
    }

    public override void Put(string path, object name, Stream stream, string contentType = null)
    {
      using (stream)
      {
        GetBlob(path, name, contentType: contentType).UploadFromStream(stream);
      }
    }

    public override void Put(string path, object name, string value, string contentType = null)
    {
      GetBlob(path, name, contentType: contentType).UploadText(value);
    }

    public override void Put(string path, object name, byte[] bytes, string contentType = null)
    {
      if (bytes == null)
      {
        DeleteIfExists(path, name);
      }
      else
      {
        using (Stream stream = new MemoryStream(bytes, writable: false))
        {
          Put(path, name, stream, contentType);
        }
      }
    }

    public override Stream Create(string path, object name, string contentType = null)
    {
      return GetBlob(path, name, contentType: contentType).OpenWrite();
    }

    public override Uri GetUri(string path, object name, AccessType type = AccessType.Public)
    {
      // TODO: determine if we are using the resizer or just serving up images directly from the cloud
      // cloud images = http://[ip]/[container]/[fileName]
      // imageResier images = /[config.prefix]/[container]/[fileName]
      // TODO: images in the path below can be defined in the imageResizer config - we need a way of finding this out
      return new Uri(string.Concat("/images/", path, "/", name), UriKind.Relative);
    }

    public override Uri GetUri(Uri baseUri, string path, object name)
    {
      return new Uri(GetBlob(path, name, false).Uri, "");
    }

    public override bool DeleteIfExists(string path, object name)
    {
      return GetBlob(path, name, false).DeleteIfExists();
    }

    public override bool Exists(string path, object name)
    {
      if (string.IsNullOrEmpty(path) || name == null || string.IsNullOrEmpty(name.ToString()))
      {
        return false;
      }

      return GetBlob(path, name, false).Exists();
    }

    private CloudBlockBlob GetBlob(string path, object name, bool createContainerIfNotExists = true, string contentType = null)
    {
      if (name == null || string.IsNullOrEmpty(name.ToString()))
      {
        throw new ArgumentNullException(nameof(name), "Blob name cannot be null");
      }

      CloudBlobContainer container = GetContainer(path, createContainerIfNotExists);
      CloudBlockBlob blob = container.GetBlockBlobReference(name.ToString());

      if (!string.IsNullOrEmpty(contentType))
      {
        blob.Properties.ContentType = contentType;
      }

      return blob;
    }

    private CloudBlobContainer GetContainer(string path, bool createIfNotExists = true)
    {
      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentNullException(nameof(path), "Container path cannot be empty");
      }

      // add regex check to see if container is
      if (!IsValidName(path))
      {
        throw new ArgumentException($"Invalid name '{path}' for container");
      }

      return GetClient().GetContainer(path, createIfNotExists);
    }

    private bool IsValidName(string name)
    {
      if (string.IsNullOrEmpty(name))
      {
        return false;
      }

      const string rootName = "$root";

      if (name.Equals(rootName))
      {
        return true;
      }

      const string pattern = @"^[a-z0-9]([a-z0-9\-]){1,61}[a-z0-9]$";

      // todo - check for consecutive hyphens

      return Regex.IsMatch(name, pattern);
    }

    private CloudBlobClient GetClient()
    {
      return _account.CreateCloudBlobClient();
    }

    private CloudStorageAccount _account;
  }
}