namespace restlessmedia.Module.File
{
  public interface IStorageProvider : IProvider
  {
    bool DeleteIfExists(string path, object name);

    bool Exists(string path, object name);

    void Put<T>(string path, object name, T obj, string contentType = null);

    T Get<T>(string path, object name);

    T GetOrDefault<T>(string path, object name, T defaultValue);
  }
}