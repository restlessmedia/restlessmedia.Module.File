using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace restlessmedia.Module.File
{
  /// <summary>
  /// <see cref="System.Net.WebClient"/> implementation of <see cref="IWebClient"/>.
  /// </summary>
  internal class SystemNetWebClient : IWebClient
  {
    public SystemNetWebClient()
    {
      _webClient = new WebClient();
    }

    public Stream OpenRead(Uri address)
    {
      return _webClient.OpenRead(address);
    }

    public Stream OpenRead(string address)
    {
      return _webClient.OpenRead(address);
    }

    public Task<Stream> OpenReadTaskAsync(Uri address)
    {
      return _webClient.OpenReadTaskAsync(address);
    }

    public void Dispose()
    {
      _webClient.Dispose();
    }

    private readonly WebClient _webClient;
  }
}