using FastMapper;
using restlessmedia.Module.Data;
using restlessmedia.Module.Data.Sql;
using System.Collections.Generic;
using System.Linq;

namespace restlessmedia.Module.File.Data
{
  public class FileSqlDataProvider : SqlDataProviderBase
  {
    public FileSqlDataProvider(IDataContext context)
      : base(context)
    { }

    public void Create(EntityType entityType, int entityId, string fileName, string mimeType, FileFlags? flags = null)
    {
      using (DatabaseContext context = CreateDatabaseContext())
      {
        FileRepository fileRepository = new FileRepository(context);
        fileRepository.Create(entityType, entityId, fileName, mimeType, flags);
        context.SaveChanges();
      }
    }

    public void Create(EntityType entityType, int entityId, ModelCollection<FileEntity> files)
    {
      if (files == null || files.Count == 0)
      {
        return;
      }

      using (DatabaseContext context = CreateDatabaseContext())
      {
        FileRepository fileRepository = new FileRepository(context);
        IEnumerable<VEntityFile> created = fileRepository.Create(entityType, entityId, files);
        context.SaveChanges();
        foreach (IFile file in created)
        {
          files.Single(x => string.Equals(x.SystemFileName, file.SystemFileName)).FileId = file.FileId;
        }
      }
    }

    public void Create(EntityType entityType, int entityId, FileEntity file)
    {
      Create(entityType, entityId, ModelCollection<FileEntity>.One(file));
    }

    public IEnumerable<string> Delete(params int[] fileIds)
    {
      using (DatabaseContext context = CreateDatabaseContext())
      {
        return new FileRepository(context).Delete(fileIds);
      }
    }

    public void Save(EntityType entityType, int entityId, FileEntity file)
    {
      using (DatabaseContext context = CreateDatabaseContext())
      {
        FileRepository fileRepository = new FileRepository(context);
        VEntityFile entityFile = fileRepository.Save(entityType, entityId, file);

        context.SaveChanges();

        if (entityFile != null)
        {
          file.FileId = entityFile.File.FileId;
        }
      }
    }

    public FileEntity Read(int fileId)
    {
      using (DatabaseContext context = CreateDatabaseContext())
      {
        FileRepository fileRepository = new FileRepository(context);
        VFile file = fileRepository.GetLicensedFile(fileId);

        if (file == null)
        {
          return null;
        }

        return ObjectMapper.Map<FileEntity>(file);
      }
    }

    public ModelCollection<FileEntity> List(EntityType entityType, int entityId)
    {
      using (DatabaseContext context = CreateDatabaseContext())
      {
        FileRepository fileRepository = new FileRepository(context);
        return new ModelCollection<FileEntity>(fileRepository.GetFiles(entityType, entityId).Select(x => ObjectMapper.Map<FileEntity>(x)));
      }
    }

    private DatabaseContext CreateDatabaseContext(bool autoDetectChanges = false)
    {
      return new DatabaseContext(DataContext, autoDetectChanges);
    }
  }
}