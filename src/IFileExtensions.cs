using System.IO;

namespace restlessmedia.Module.File
{
  public static class IFileExtensions
  {
    public static string SystemFileNameNoExtension(this IFile file)
    {
      return Path.GetFileNameWithoutExtension(file.SystemFileName);
    }

    public static string ExtensionNoPeriod(this IFile file)
    {
      string extension = Extension(file);

      if (string.IsNullOrEmpty(extension))
      {
        return null;
      }

      const char period = '.';

      return extension.TrimStart(new char[] { period });
    }

    public static bool HasFileName(this IFile file)
    {
      return !string.IsNullOrEmpty(file.FileName);
    }

    /// <summary>
    /// File extension including period
    /// </summary>
    public static string Extension(this IFile file)
    {
      if (HasFileName(file))
      {
        return Path.GetExtension(file.FileName);
      }

      return null;
    }
  }
}