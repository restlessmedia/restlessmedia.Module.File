using restlessmedia.Module.Extensions;
using restlessmedia.Module.File.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace restlessmedia.Module.File
{
  public class FileSystemStorageProvider : IDiskStorageProvider
  {
    public FileSystemStorageProvider(IFileSettings fileSettings)
    {
      _fileSettings = fileSettings ?? throw new ArgumentNullException(nameof(fileSettings));
    }

    public virtual byte[] Get(string path, object name)
    {
      return System.IO.File.ReadAllBytes(GetPath(path, name));
    }

    public virtual T Get<T>(string path, object name)
    {
      Stream stream;

      if (Exists(path, name, out stream))
      {
        return Deserialize<T>(stream);
      }

      return default(T);
    }

    public virtual T GetOrDefault<T>(string path, object name, T defaultValue)
    {
      return Exists(path, name) ? Get<T>(path, name) : defaultValue;
    }

    public virtual Stream Stream(string path, object name)
    {
      return System.IO.File.OpenRead(GetPath(path, name));
    }

    public virtual void Put<T>(string path, object name, T obj, string contentType = null)
    {
      using (FileStream fileStream = System.IO.File.OpenWrite(GetPath(path, name)))
      {
        Serialize(obj).CopyTo(fileStream);
      }
    }

    public virtual void Put(string path, object name, byte[] bytes, string contentType = null)
    {
      if (bytes != null)
      {
        System.IO.File.WriteAllBytes(GetPath(path, name), bytes);
      }
      else
      {
        Put(path, name, string.Empty, contentType);
      }
    }

    public virtual void Put(string path, object name, Stream stream, string contentType = null)
    {
      if (stream != null)
      {
        using (Stream fileStream = System.IO.File.Create(GetPath(path, name)))
        {
          stream.CopyTo(fileStream);
        }
      }
      else
      {
        Put(path, name, string.Empty, contentType);
      }
    }

    public virtual void Put(string path, object name, string value, string contentType = null)
    {
      System.IO.File.WriteAllText(GetPath(path, name), value);
    }

    public virtual Stream Create(string path, object name, string contentType = null)
    {
      return System.IO.File.OpenWrite(GetPath(path, name));
    }

    public virtual Uri GetUri(string path, object name, AccessType type = AccessType.Public)
    {
      IDiskAccess access = _fileSettings.GetAccess(type);

      if (access == null)
      {
        throw new KeyNotFoundException($"'{type}' file storage not configured");
      }

      return GetUri(access.BaseUri, path, name);
    }

    public virtual Uri GetUri(Uri baseUri, string path, object name)
    {
      if (baseUri == null)
      {
        throw new ArgumentNullException(nameof(baseUri), "Attempting to use a null uri for GetUri");
      }

      if (baseUri.IsAbsoluteUri)
      {
        return new Uri(baseUri, GetPath(path, name));
      }
      else
      {
        // Creating a new uri from 2 relative uris doesn't work so well
        string uri = string.Concat(baseUri.OriginalString, GetPath(path, name));

        try
        {
          return new Uri(uri, UriKind.Relative);
        }
        catch (Exception e)
        {
          throw new ArgumentException("BaseUri", $"Could not create Uri from '{uri}' for GetUri(). {e.Message}");
        }
      }
    }

    public virtual bool DeleteIfExists(string path, object name)
    {
      System.IO.File.Delete(GetPath(path, name));
      return true;
    }

    public virtual bool Exists(string path, object name)
    {
      return System.IO.File.Exists(GetPath(path, name));
    }

    public virtual bool Exists(string path, object name, out Stream stream)
    {
      stream = null;

      if (Exists(path, name))
      {
        stream = System.IO.File.OpenRead(GetPath(path, name));
        return true;
      }

      return false;
    }

    protected static Stream Serialize<T>(T obj)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize(memoryStream, obj);
        return memoryStream;
      }
    }

    protected static T Deserialize<T>(Stream stream)
    {
      return (T)new BinaryFormatter().Deserialize(stream);
    }

    protected static T Deserialize<T>(byte[] data)
    {
      if (data == null)
      {
        return default(T);
      }

      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        return Deserialize<T>(memoryStream);
      }
    }

    private string GetPath(string path, object name)
    {
      string nameAsString = name?.ToString();

      // replace unsupported chars
      if (!string.IsNullOrEmpty(nameAsString))
      {
        nameAsString = nameAsString.ReplaceAll(_fileSettings.FileNameCharacterBlackList, string.Empty);
      }

      if (_fileSettings.FileNameBlackList.Contains(nameAsString))
      {
        nameAsString = string.Join("_", nameAsString.Select(x => x));
      }

      return string.Concat(path, nameAsString);
    }

    private readonly IFileSettings _fileSettings;
  }
}