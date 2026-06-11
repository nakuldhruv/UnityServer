using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tcp
{
    internal class TcpServerV2
    {
        private bool _isRunning;
        private TcpListener _listener;
        private readonly List<TcpClient> _clients = new List<TcpClient>();

        public TcpServerV2(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            _isRunning = true;

            while (_isRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                lock (_clients)
                {
                    _clients.Add(client);
                }

                _ = HandleClientAsync(client);
            }
        }

        public async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = null;
            byte[] lengthBuffer = new byte[4];
            stream = client.GetStream();
            while (_isRunning)
            {
                int bytesRead = 0;
                while (bytesRead < lengthBuffer.Length)
                {
                    int read = await stream.ReadAsync(lengthBuffer, bytesRead, lengthBuffer.Length - bytesRead);
                    if(read == 0) return;
                    bytesRead += read;
                }
            
                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                if (messageLength <= 0 || messageLength > 1024 * 1024)
                {
                    return;
                }
                byte[] messageBuffer = new byte[messageLength];
                bytesRead = 0;
                while (bytesRead < messageBuffer.Length)
                {
                    int read = await stream.ReadAsync(messageBuffer, bytesRead, messageBuffer.Length - bytesRead);
                    if(read == 0) return;
                    bytesRead += read;
                }

                string message = Encoding.UTF8.GetString(messageBuffer);
                Console.WriteLine(message);   
            }
        }
    }
}
