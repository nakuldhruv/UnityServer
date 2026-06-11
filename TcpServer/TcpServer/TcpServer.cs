using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tcp
{
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
                byte[] lenBuffer = new byte[4];
                byte[] msgBuffer;
                while (true)
                {
                    int bytesRead = 0;
                    while (bytesRead < lenBuffer.Length)
                    {
                        int n = await stream.ReadAsync(lenBuffer, bytesRead, 4 - bytesRead);
                        if (n == 0) return;
                        bytesRead += n;
                    }

                    int msgLen = BitConverter.ToInt32(lenBuffer, 0);
                    if (msgLen <= 0 || msgLen > 1024 * 1024)
                    {
                        Console.WriteLine("消息长度非法");
                        return;
                    }

                    msgBuffer = new byte[msgLen];
                    bytesRead = 0;
                    while (bytesRead < msgLen)
                    {
                        int n = await stream.ReadAsync(msgBuffer, bytesRead, msgLen - bytesRead);
                        if (n == 0) return;
                        bytesRead += n;
                    }

                    string msg = Encoding.UTF8.GetString(msgBuffer, 0, msgLen);
                    Console.WriteLine(msg);

                    byte[] echoData = Encoding.UTF8.GetBytes(msg);
                    byte[] echoLen = BitConverter.GetBytes(echoData.Length);
                    await stream.WriteAsync(echoLen, 0, echoLen.Length);
                    await stream.WriteAsync(echoData, 0, echoData.Length);
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
    }
}