using System;
using System.Collections.Generic;
using System.Linq;

namespace restlessmedia.Module.File
{
  public static class MimeExtensions
  {
    /// <summary>
    /// Returns the extension for a given type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetExtension(string type)
    {
      if (string.IsNullOrEmpty(type))
      {
        throw new ArgumentNullException(nameof(type));
      }

      if (!Extensions.ContainsValue(type))
      {
        return null;
      }

      return Extensions.First(x => string.Compare(x.Value, type, true) == 0).Key;
    }

    /// <summary>
    /// Returns the mime type for a given extension
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    public static string GetMimeType(string extension)
    {
      if (extension.StartsWith("."))
      {
        extension = extension.TrimStart('.');
      }

      if (!Extensions.ContainsKey(extension))
      {
        return null;
      }

      return Extensions[extension.ToLower()];
    }

    public static Dictionary<string, string> Extensions = new Dictionary<string, string>
		{
			{ "3g2", "video/3gpp2" },
			{ "3gp", "video/3gpp" },
			{ "7z", "application/x-7z-compressed" },
			{ "avi", "video/x-msvideo" },
			{ "bmp", "image/bmp" },
			{ "css", "text/css" },
			{ "csv", "text/csv" },
			{ "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
			{ "dot", "application/msword" },
			{ "dotm", "application/vnd.ms-word.template.macroenabled.12" },
			{ "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
			{ "dump", "application/octet-stream" },
			{ "dvi", "application/x-dvi" },
      { "gif", ContentType.Gif },
			{ "htm", "text/html" },
			{ "html", "text/html" },
			{ "jpg", ContentType.Jpeg },
      { "jpeg", ContentType.Jpeg },
      { "jpe", ContentType.Jpeg },
			{ "jpgm", "video/jpm" },
			{ "jpgv", "video/jpeg" },
			{ "jpm", "video/jpm" },
			{ "js", "application/javascript" },
			{ "json", ContentType.Json },
			{ "m1v", "video/mpeg" },
			{ "mov", "video/quicktime" },
			{ "mp4", "video/mp4" },
			{ "mp4a", "audio/mp4" },
			{ "mpeg", "video/mpeg" },
			{ "mpg", "video/mpeg" },
			{ "mpg4", "video/mp4" },
			{ "pdf", "application/pdf" },
			{ "png", ContentType.Png },
			{ "ppt", "application/vnd.ms-powerpoint" },
			{ "pptm", "application/vnd.ms-powerpoint.presentation.macroenabled.12" },
			{ "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
			{ "rar", "application/x-rar-compressed" },
			{ "tif", "image/tiff" },
			{ "tiff", "image/tiff" },
			{ "txt", ContentType.Text },
			{ "wav", "audio/x-wav" },
			{ "xhtml", ContentType.XHtml },
			{ "xls", "application/vnd.ms-excel" },
			{ "xml", ContentType.XML },
			{ "xsl", "application/xml" },
			{ "xslt", "application/xslt+xml" },
			{ "zip", "application/zip" },
		};
  }
}