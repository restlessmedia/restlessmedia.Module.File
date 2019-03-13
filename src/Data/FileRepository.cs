using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using restlessmedia.Module.Data.EF;
using restlessmedia.Module.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace restlessmedia.Module.File.Data
{
  public class FileRepository : LicensedEntityRepository<VFile>
  {
    public FileRepository(DatabaseContext context)
      : base(context)
    {
      _databaseContext = context;
    }

    /// <summary>
    /// Returns an entity file.  Doesn't check license.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="entityId"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public VEntityFile GetEntityFile(EntityType entityType, int entityId, FileEntity file)
    {
      return _databaseContext
          .EntityFile
          .Include(x => x.File)
          .FirstOrDefault(x => x.Entity.EntityType == entityType && x.Entity.EntityId == entityId && string.Compare(x.File.FileName, file.FileName, true) == 0);
    }

    /// <summary>
    /// Returns a list of related files for a given entity.  Doesn't check license, presume that the calling entity has already passed the check for license and can see these files.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public IEnumerable<VFile> GetFiles(EntityType entityType, int entityId)
    {
      return _databaseContext
          .EntityFile
          .Include(x => x.File)
          .Where(x => x.Entity.EntityType == entityType && x.Entity.EntityId == entityId)
          .OrderBy(x => x.Rank == null)
          .ThenBy(x => x.Rank)
          .ThenBy(x => x.File.SystemFileName)
          .Select(x => x.File);
    }

    public VEntityFile Create(EntityType entityType, int entityId, string fileName, string mimeType, FileFlags? flags = null)
    {
      TEntity entity = GetEntity(entityType, entityId);

      if (entity == null)
      {
        ThrowEntityNotFoundException(entityType, entityId);
      }

      return _databaseContext.EntityFile.Add(new VEntityFile(entity, new VFile(fileName, mimeType, fileName) { Flags = (int?)flags }));
    }

    public IEnumerable<VEntityFile> Create(EntityType entityType, int entityId, IEnumerable<FileEntity> files)
    {
      TEntity entity = GetEntity(entityType, entityId);

      if (entity == null)
      {
        ThrowEntityNotFoundException(entityType, entityId);
      }

      return _databaseContext.EntityFile.AddRange(files.Select(x => new VEntityFile(entity, new VFile(x))));
    }

    public VEntityFile Save(EntityType entityType, int entityId, FileEntity file, Action<VEntityFile> onUpdate = null)
    {
      TEntity entity = GetEntity(entityType, entityId);

      if (entity == null)
      {
        ThrowEntityNotFoundException(entityType, entityId);
      }

      VEntityFile entityFile = GetEntityFile(entityType, entityId, file);

      if (entityFile == null)
      {
        VFile vfile = new VFile(file);
        entityFile = new VEntityFile(entity, vfile);
        _databaseContext.EntityFile.Add(entityFile);
      }
      else
      {
        onUpdate?.Invoke(entityFile);
      }

      return entityFile;
    }

    public VFile GetLicensedFile(int fileId)
    {
      return _databaseContext
        .EntityFile
        .Include(x => x.File)
        .Where(x => x.Entity.LicenseId == Context.LicenseId && x.File.FileId == fileId)
        .Select(x => x.File)
        .SingleOrDefault();
    }

    public IEnumerable<string> Delete(params int[] fileIds)
    {
      const string sql = "dbo.SPDeleteFiles @licenseId, @ids";
      return _databaseContext.Database.SqlQuery<string>(sql,
         new SqlParameter("@licenseId", SqlDbType.Int) { Value = Context.LicenseId },
        UDTHelper.CreateParameter("@ids", fileIds)
      ).ToList();
    }

    private readonly DatabaseContext _databaseContext;
  }
}