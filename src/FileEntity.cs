using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace restlessmedia.Module.File
{
  [Serializable]
  public class FileEntity : Entity, IFile
  {
    public int? FileId { get; set; }

    public virtual string FileName { get; set; }

    public virtual string SystemFileName { get; set; }

    /// <summary>
    /// Used for storing custom flags against a file, typically, this would store the value of an enum
    /// </summary>
    public virtual int? Flags { get; set; }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.File;
      }
    }

    public override int? EntityId
    {
      get
      {
        return FileId;
      }
    }

    public virtual string MimeType
    {
      get
      {
        if (string.IsNullOrEmpty(_mimeType) && !string.IsNullOrEmpty(this.Extension()))
        {
          _mimeType = MimeExtensions.GetMimeType(this.ExtensionNoPeriod());
        }

        return _mimeType;
      }
      set
      {
        _mimeType = value;
      }
    }

    public virtual FileType FileType
    {
      get
      {
        return new string[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(this.Extension()) ? FileType.Image : FileType.File;
      }
    }

    public virtual DateTime? LastUpdated { get; set; }

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

    /// <summary>
    /// Used for dapper when file appears in select with other entities using title column
    /// </summary>
    private string File
    {
      get
      {
        return base.Title;
      }
      set
      {
        base.Title = value;
      }
    }

    private string _mimeType = null;
  }
}