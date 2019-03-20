using FastMember;
using restlessmedia.Module.Attributes;
using restlessmedia.Module.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace restlessmedia.Module.File
{
  /// <summary>
  /// A collection of helpers for downloading files.
  /// </summary>
  public class FileHelper
	{
    /// <summary>
    /// Resizes an image at the path to the given dimensions.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="maintainAspectRatio"></param>
    /// <returns></returns>
		public Image Resize(string path, int width, int height, bool maintainAspectRatio)
		{
      if (!System.IO.File.Exists(path))
      {
        throw new ArgumentException($"The file \'{path}\' doesn't exist.", nameof(path));
      }

      Image image = Image.FromFile(path);
			return image.Resize(width, height, maintainAspectRatio);
		}

		public Image CropImage(string path, int x, int y, int width, int height)
		{
			Image image = Image.FromFile(path);
			return image.Crop(x, y, width, height);
		}

    /// <summary>
    /// Returns all filenames for a given path containing a specific keyword.  Caution, use sparingly.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public string[] GetFilesContaining(string path, string keyword)
    {
      return Directory.EnumerateFiles(path).Where(x => Regex.IsMatch(System.IO.File.ReadAllText(x), keyword, RegexOptions.IgnoreCase | RegexOptions.Multiline)).ToArray();
    }

    /// <summary>
    /// Returns all filenames for a given path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
		public string[] GetFiles(string path)
		{
			return GetFiles(path, "*");
		}

    /// <summary>
    /// Returns all filenames with the specified pattern for a given path.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
		public string[] GetFiles(string path, string pattern)
		{
			if (!Directory.Exists(path))
      {
        throw new DirectoryNotFoundException($"The directory \"{path}\" doesn't exist.");
      }

      return Directory.GetFiles(path, pattern);
		}

		public string[] ReadFile(string path)
		{
			if (!System.IO.File.Exists(path))
      {
        throw new DirectoryNotFoundException($"The file \"{path}\" doesn't exist.");
      }

      return System.IO.File.ReadAllLines(path);
		}

		public FileInfo GetMostRecentFile(string directory)
		{
			return GetMostRecentFile(directory, "*");
		}

		public FileInfo GetMostRecentFile(string directory, string pattern)
		{
			if (!Directory.Exists(directory))
      {
        throw new DirectoryNotFoundException(directory);
      }

      DirectoryInfo directoryInfo = new DirectoryInfo(directory);
			FileInfo file = directoryInfo.GetFiles(pattern).OrderByDescending(f => f.LastWriteTime).First();

			return file;
		}

		public void DeleteFiles(string directory, IEnumerable<string> files, bool throwOnError = false)
		{
			foreach (string file in files)
			{
				string path = Path.Combine(directory, file);
				if (throwOnError)
				{
					try
					{
						System.IO.File.Delete(path);
					}
					catch (Exception)
					{
						throw;
					}
				}
				else
				{
					System.IO.File.Delete(path);
				}
			}
		}

		public string[] DeleteFiles(string directory, string pattern, bool throwOnError = false)
		{
			string[] filesToDelete = GetFiles(directory, pattern);
			foreach (string file in filesToDelete)
			{
				try
				{
					System.IO.File.Delete(file);
				}
				catch (Exception)
				{
					if (throwOnError)
					{
						throw;
					}
				}
			}

			return filesToDelete;
		}

		/// <summary>
		/// Returns the image format and content type for an image extension
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public System.Tuple<ImageFormat, string> GetImageInfo(string extension)
		{
			if (extension.StartsWith("."))
      {
        extension = extension.Trim('.');
      }

      return Tuple.Create<ImageFormat, string>(GetImageFormat(extension), MimeExtensions.GetMimeType(extension));
		}

		/// <summary>
		/// Returns the image format for an image extension
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public ImageFormat GetImageFormat(string extension)
		{
			switch (extension.ToLower())
			{
				case "gif":
					return ImageFormat.Gif;
				case "jpg":
				case "jpe":
				case "jpeg":
					return ImageFormat.Jpeg;
				case "png":
					return ImageFormat.Png;
				case "bmp":
					return ImageFormat.Bmp;
				default:
					throw new ArgumentException($"Unable to get image format from extension '{extension}'.", nameof(extension));
			}
		}

    /// <summary>
    /// Writes a delimited value string to the specified stream
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="stream"></param>
    /// <param name="separator"></param>
    /// <param name="padChar"></param>
    /// <param name="newLine"></param>
    public void WriteCSV<T>(IEnumerable<T> list, Stream stream, string separator = ",", string padChar = "\"", string newLine = "\r\n")
    {
      IEnumerable<Member> members = AttributeHelper.Filter<T, IgnoreAttribute>(isDefined: false);

      using (StreamWriter writer = new StreamWriter(stream))
      {
        // header row
        writer.Write(string.Join(separator, members.Select(x => CsvFormat(x.Name, padChar, newLine)).ToArray()));
        writer.Write(newLine);

        // data rows
        foreach (T obj in list)
        {
          ObjectAccessor objectAccessor = ObjectAccessor.Create(obj);
          writer.Write(string.Join(separator, members.Select(x => CsvFormat(objectAccessor[x.Name], padChar, newLine)).ToArray()));
          writer.Write(newLine);
        }
      }
    }

    private string CsvFormat(object value, string padChar, string newLine)
    {
      if (value == null)
      {
        return null;
      }

      return value.ToString().Replace(newLine, " ").Pad(padChar);
    }
	}
}