using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace CSharpCourse.DesignPatterns.Structural.Facade;

internal class MyHttpClient : IDisposable
{
    private TcpClient? _tcpClient;
    private Stream? _commonStream;
    private NetworkStream? _networkStream;
    private bool _disposed;

    public static string GetIpAddress(string domain)
    {
        var addresses = Dns.GetHostAddresses(domain);
        return addresses[0].ToString();
    }

    public async Task ConnectAsync(string ip, int port, bool ssl = true)
    {
        _tcpClient = new TcpClient();
        await _tcpClient.ConnectAsync(ip, port);

        _networkStream = _tcpClient.GetStream();

        if (ssl)
        {
            var sslOptions = new SslClientAuthenticationOptions
            {
                TargetHost = ip,
                RemoteCertificateValidationCallback = new((_, _, _, _) => true)
            };

            var sslStream = new SslStream(_networkStream);
            await sslStream.AuthenticateAsClientAsync(sslOptions);

            _commonStream = sslStream;
        }
        else
        {
            _commonStream = _networkStream;
        }
    }

    public async Task<string> SendGetRequestAsync(string host, string uri)
    {
        var sb = new StringBuilder()
            .AppendLine($"GET {uri} HTTP/1.1")
            .AppendLine($"Host: {host}")
            .AppendLine("Connection: close")
            .AppendLine();

        var request = Encoding.ASCII.GetBytes(sb.ToString());
        await _commonStream!.WriteAsync(request, 0, request.Length);

        var response = new byte[4096];
        var responseBuilder = new StringBuilder();

        do
        {
            var bytesRead = await _commonStream.ReadAsync(response, 0, response.Length);
            responseBuilder.Append(Encoding.ASCII.GetString(response, 0, bytesRead));
        } while (_networkStream!.DataAvailable);

        return responseBuilder.ToString();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // If already disposed, don't dispose again
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed
            _tcpClient?.Dispose();
            _commonStream?.Dispose();
            _networkStream?.Dispose();
        }

        // Dispose unmanaged

        _disposed = true;
    }

    ~MyHttpClient()
    {
        Dispose(false);
    }
}
