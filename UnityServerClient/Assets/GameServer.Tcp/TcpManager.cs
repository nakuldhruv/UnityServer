using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tcp
{
    public class TcpManager : MonoBehaviour
    {
        private string                  _ip   = "127.0.0.1";
        private int                     _port = 8888;
        private TcpClient               _client;
        private NetworkStream           _stream;
        private bool                    _isConnected  = false;
        private ConcurrentQueue<string> _messageQueue = new();

        private async void Awake()
        {
            await Connect();
            await Send("Tcp");
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

        public async Task Send(string message)
        {
            if(!_isConnected || _stream == null) return;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            try
            {
                await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }

        public void Disconnect(string reason = "主动断开")
        {
            if(!_isConnected) return;
            _isConnected = false;
            _stream?.Close();
            _client?.Close();
            Debug.Log(reason);
        }
        
        private async Task ReceiveLoop()
        {
            byte[] buffer = new byte[4096];
            while (_isConnected && _client.Connected)
            {
                try
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        Disconnect();
                        break;
                    }
                    
                    string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    _messageQueue.Enqueue(text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        private void Update()
        {
            while (_messageQueue.TryDequeue(out string message))
            {
                Debug.Log(message);
            }
        }
    }
}