namespace System.IO
{
  public static class IOExtensions
  {
    /// <summary>
    /// Creates a re-usable memory stream from another.  This must be handled and closed by the caller.
    /// </summary>
    /// <param name="inputStream"></param>
    /// <returns></returns>
    public static MemoryStream Copy(this Stream inputStream)
    {
      const int readSize = 256;
      byte[] buffer = new byte[readSize];

      MemoryStream memoryStream = new MemoryStream();

      using (inputStream)
      {
        int count = inputStream.Read(buffer, 0, readSize);
        while (count > 0)
        {
          memoryStream.Write(buffer, 0, count);
          count = inputStream.Read(buffer, 0, readSize);
        }
      }

      // reset it back to the start
      memoryStream.Position = 0;

      return memoryStream;
    }

    public static void WriteTo(this MemoryStream stream, string path)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
      {
        stream.Position = 0;
        stream.WriteTo(fileStream);
      }
    }

    public static byte[] ReadAllBytes(this Stream stream, int bufferSize = 16 * 1024)
    {
      byte[] buffer = new byte[bufferSize];

      using (MemoryStream memoryStream = new MemoryStream())
      {
        int read;
        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
          memoryStream.Write(buffer, 0, read);
        }
        return memoryStream.ToArray();
      }
    }

    public static string ReadAll(this Stream stream)
    {
      using (stream)
      {
        return new StreamReader(stream).ReadToEnd();
      }
    }
  }
}