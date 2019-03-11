using restlessmedia.Module.File.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace restlessmedia.Module.File
{
  public class FileService : IFileService
  {
    public FileService(IEntityService entityService, IFileDataProvider fileDataProvider, IDiskStorageProvider storageProvider)
    {
      _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
      _fileDataProvider = fileDataProvider ?? throw new ArgumentNullException(nameof(fileDataProvider));
      _storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
    }

    public void Create(EntityType entityType, int entityId, string fileName, string mimeType = null, FileFlags? flags = null)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException(nameof(fileName));
      }

      if (mimeType == null)
      {
        mimeType = MimeExtensions.GetMimeType(Path.GetExtension(fileName));
      }

      _fileDataProvider.Create(entityType, entityId, fileName, mimeType, flags);
    }

    public void Save(EntityType entityType, int entityId, FileEntity file)
    {
      _fileDataProvider.Save(entityType, entityId, file);
    }

    public FileEntity Read(int fileId)
    {
      return _fileDataProvider.Read(fileId);
    }

    public ModelCollection<FileEntity> List(EntityType entityType, int entityId)
    {
      return _fileDataProvider.List(entityType, entityId);
    }

    public void Delete(string path, params string[] files)
    {
      foreach (string file in files)
      {
        _storageProvider.DeleteIfExists(path, file);
      }
    }

    public void Delete(string path, string file)
    {
      _storageProvider.DeleteIfExists(path, file);
    }

    public void Delete(string path, params int[] fileIds)
    {
      IEnumerable<string> files = _fileDataProvider.Delete(fileIds);

      foreach (string file in files)
      {
        _storageProvider.DeleteIfExists(path, file);
      }
    }

    public Uri GetUri(string path, string file)
    {
      return _storageProvider.GetUri(path, file);
    }

    public Uri GetUri(Uri baseUri, string path, string file)
    {
      return _storageProvider.GetUri(baseUri, path, file);
    }

    private readonly IEntityService _entityService;

    private readonly IFileDataProvider _fileDataProvider;

    private readonly IDiskStorageProvider _storageProvider;
  }
}