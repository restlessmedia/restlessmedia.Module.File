using System;
using System.IO;
using System.Threading.Tasks;

namespace restlessmedia.Module.File
{
  public interface IWebClient : IDisposable
  {
    Stream OpenRead(Uri address);

    Stream OpenRead(string address);

    Task<Stream> OpenReadTaskAsync(Uri address);
  }
}