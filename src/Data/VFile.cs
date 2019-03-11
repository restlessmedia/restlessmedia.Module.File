using restlessmedia.Module.Data.EF;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restlessmedia.Module.File.Data
{
  [Table("VFile")]
  public class VFile : LicensedEntity, IFile
  {
    public VFile() { }

    public VFile(IFile file)
    {
      if (file == null)
      {
        return;
      }

      FileId = file.FileId;
      FileName = file.FileName;
      SystemFileName = file.SystemFileName;
      MimeType = file.MimeType;
      Flags = file.Flags;
      LastUpdated = file.LastUpdated;
    }

    public VFile(FileEntity file)
    {
      if (file.FileId.HasValue)
      {
        FileId = file.FileId.Value;
      }

      FileName = file.FileName;
      MimeType = file.MimeType;
      SystemFileName = file.SystemFileName;
      Flags = file.Flags;
    }

    public VFile(string fileName, string mimeType, string systemFileName)
    {
      FileName = fileName;
      MimeType = mimeType;
      SystemFileName = systemFileName;
    }

    public VFile(int fileId, string fileName, string mimeType, string systemFileName)
      : this(fileName, mimeType, systemFileName)
    {
      FileId = fileId;
    }

    [Key]
    public int? FileId { get; set; }

    [Varchar]
    public string FileName { get; set; }

    [Varchar]
    public string SystemFileName { get; set; }

    [Varchar]
    public string MimeType { get; set; }

    public int? Flags { get; set; }

    public override int? EntityId
    {
      get
      {
        return FileId.GetValueOrDefault(0);
      }
    }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.File;
      }
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? LastUpdated { get; set; }

    [NotMapped]
    public override string Title
    {
      get
      {
        return FileName;
      }
      set
      {
        FileName = value;
      }
    }
  }
}