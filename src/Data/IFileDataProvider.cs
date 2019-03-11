using restlessmedia.Module.Data;
using System.Collections.Generic;

namespace restlessmedia.Module.File.Data
{
  public interface IFileDataProvider : IDataProvider
  {
    void Create(EntityType entityType, int entityId, string fileName, string mimeType, FileFlags? flags = null);

    void Create(EntityType entityType, int entityId, ModelCollection<FileEntity> files);

    void Create(EntityType entityType, int entityId, FileEntity file);

    void Save(EntityType entityType, int entityId, FileEntity file);

    IEnumerable<string> Delete(params int[] fileIds);

    FileEntity Read(int fileId);

    ModelCollection<FileEntity> List(EntityType entityType, int entityId);
  }
}