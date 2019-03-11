using System;
using System.IO;

namespace restlessmedia.Module.File
{
  public interface IDiskStorageProvider : IStorageProvider
  {
    byte[] Get(string path, object name);

    Stream Stream(string path, object name);

    void Put(string path, object name, byte[] bytes, string contentType = null);

    void Put(string path, object name, Stream stream, string contentType = null);

    void Put(string path, object name, string value, string contentType = null);

    Stream Create(string path, object name, string contentType = null);

    /// <summary>
    /// Returns the Uri for the location of an item
    /// </summary>
    /// <remarks>This routine needs to be as light as possible</remarks>
    /// <param name="location"></param>
    /// <returns></returns>
    Uri GetUri(string path, object name, AccessType type = AccessType.Public);

    /// <summary>
    /// Returns the Uri for the location of an item
    /// </summary>
    /// <remarks>This routine needs to be as light as possible</remarks>
    /// <param name="baseUri"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    Uri GetUri(Uri baseUri, string path, object name);
  }
}