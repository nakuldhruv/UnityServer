using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpServer
{
    private TcpListener _listener;
    private readonly List<TcpClient> _clients = new List<TcpClient>();
    private bool _isRunning;

    public TcpServer(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task StartAsync()
    {
        _listener.Start();
        _isRunning = true;
        Console.WriteLine($"服务器已启动，监听端口: {((IPEndPoint)_listener.LocalEndpoint).Port}");

        while (_isRunning)
        {
            try
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                lock (_clients) _clients.Add(client);
                Console.WriteLine($"客户端已连接: {client.Client.RemoteEndPoint}");
                _ = HandleClientAsync(client); // 不等待，并发处理
            }
            catch (Exception ex)
            {
                Console.WriteLine($"接受客户端异常: {ex.Message}");
            }
        }
    }
    private async Task HandleClientAsync(TcpClient client)
    {
        NetworkStream stream = null;
        try
        {
            stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                // 收到数据，转成字符串（假设UTF8编码，且每条消息以换行符结尾）
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"[{client.Client.RemoteEndPoint}] 收到: {msg}");

                // 回显（Echo）
                byte[] echo = Encoding.UTF8.GetBytes($"Echo: {msg}");
                await stream.WriteAsync(echo, 0, echo.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"异常: {ex.Message}");
        }
        finally
        {
            // 手动关闭和释放资源
            stream?.Close();
            client.Close();
            Console.WriteLine($"客户端断开: {client.Client.RemoteEndPoint}");
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _listener?.Stop();
        lock (_clients)
        {
            foreach (var client in _clients)
                client?.Close();
            _clients.Clear();
        }
        Console.WriteLine("服务器已停止");
    }

    // 主入口
    public static async Task Main(string[] args)
    {
        int port = 8888; // 默认端口
        if (args.Length > 0 && int.TryParse(args[0], out int p))
            port = p;

        var server = new TcpServer(port);
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            server.Stop();
        };

        await server.StartAsync();
    }
}