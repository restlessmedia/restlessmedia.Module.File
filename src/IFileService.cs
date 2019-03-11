using System;

namespace restlessmedia.Module.File
{
  public interface IFileService : IService
  {
    void Create(EntityType entityType, int entityId, string fileName, string mimeType = null, FileFlags? flags = null);

    void Save(EntityType entityType, int entityId, FileEntity file);

    /// <summary>
    /// Returns a file
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    FileEntity Read(int fileId);

    void Delete(string path, params string[] files);

    void Delete(string path, params int[] fileIds);

    /// <summary>
    /// Lists files for a given entity
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="entityId"></param>
    /// <returns></returns>
    ModelCollection<FileEntity> List(EntityType entityType, int entityId);

    Uri GetUri(string path, string file);

    Uri GetUri(Uri baseUri, string path, string file);
  }
}