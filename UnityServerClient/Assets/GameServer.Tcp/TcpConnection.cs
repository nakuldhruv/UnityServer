using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tcp
{
    public class TcpConnection : MonoBehaviour
    {
        private string                  _ip   = "127.0.0.1";
        private int                     _port = 8888;
        private TcpClient               _client;
        private NetworkStream           _stream;
        private bool                    _isConnected;
        private ConcurrentQueue<string> _messageQueue = new();

        private async void Awake()
        {
            await Connect();
            _ = Send("Tcp client send message.");
        }
        
        private void Update()
        {
            while (_messageQueue.TryDequeue(out string message))
            {
                Debug.Log(message);
            }
        }

        public async Task Connect()
        {
            if (_isConnected)
            {
                Debug.Log("Already connected");
                return;
            }

            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_ip, _port);
                _stream      = _client.GetStream();
                _isConnected = true;
                _            = ReceiveLoop();
                Debug.Log("Connected");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }

        public void Disconnect()
        {
            if(!_isConnected) return;
            _isConnected = false;
            _stream?.Close();
            _client?.Close();
            Debug.Log("Disconnected");
        }
        
        private async Task ReceiveLoop()
        {
            byte[] lenBuffer = new byte[4];
            while (_isConnected && _client.Connected)
            {
                try
                {
                    int bytesRead = 0;
                    while (bytesRead < 4)
                    {
                        int n = await _stream.ReadAsync(lenBuffer, bytesRead, 4 - bytesRead);
                        if (n == 0)
                        {
                            Disconnect();
                            return;
                        }
                        
                        bytesRead += n;
                    }
                    
                    int msgLen =  BitConverter.ToInt32(lenBuffer, 0);
                    if (msgLen <= 0 || msgLen > 1024 * 1024)
                    {
                        Disconnect();
                        return;
                    }
                    
                    byte[] msgBuffer = new byte[msgLen];
                    bytesRead = 0;
                    while (bytesRead < msgLen)
                    {
                        int n = await _stream.ReadAsync(msgBuffer, bytesRead, msgLen - bytesRead);
                        if (n == 0)
                        {
                            Disconnect();
                            return;
                        }
                        
                        bytesRead += n;
                    }
                    
                    string text = Encoding.UTF8.GetString(msgBuffer, 0, msgLen);
                    _messageQueue.Enqueue(text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        
        public async Task Send(string message)
        {
            if(!_isConnected || _stream == null) return;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            int len = messageBytes.Length;
            byte[] lenBytes = BitConverter.GetBytes(len);
            try
            {
                await _stream.WriteAsync(lenBytes, 0, lenBytes.Length);
                await _stream.WriteAsync(messageBytes, 0, len);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }
    }
}