using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tcp
{
    public class TcpManagerV2 : MonoBehaviour
    {
        [SerializeField] private string _ip   = "127.0.0.1";
        [SerializeField] private int    _port = 8888;

        private volatile bool          _isConnected;
        private          TcpClient     _client;
        private          NetworkStream _stream;
        
        private readonly ConcurrentQueue<string> _messageQueue = new();
        private readonly int                     _messageSize  = 1024 * 1024;

        private async void Start()
        {
            await ConnectAsync();
            if (_isConnected)
            {
                await SendAsync("客户端连接成功：发送消息");   
            }
        }

        private void Update()
        {
            while (_messageQueue.TryDequeue(out var message))
            {
            }
        }

        private void OnDestroy()
        {
            _ = DisconnectAsync();
        }

        private async Task ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_ip, _port);
                _stream      = _client.GetStream();
                _isConnected = true;
                _            = Task.Run(ReceiveLoopAsync);
                Debug.Log($"{nameof(ConnectAsync)}连接服务器成功{_ip}:{_port}");
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(ConnectAsync)}连接服务器失败{_ip}:{_port}_{e.Message}");
            }
        }

        private Task DisconnectAsync()
        {
            _client?.Close();
            _client      = null;
            _stream      = null;
            _isConnected = false;
            return Task.CompletedTask;
        }

        private async Task ReceiveLoopAsync()
        {
            byte[] lengthBuffer = new byte[4];
            while (_isConnected)
            {
                try
                {
                    int bytesRead = 0;
                    while (bytesRead < 4) // 读取消息体长度
                    {
                        int read = await _stream.ReadAsync(lengthBuffer, bytesRead, 4 - bytesRead);
                        if (read == 0)
                        {
                            Debug.LogWarning("TCP 连接已关闭（读取消息长度前缀时对端主动断开）");
                            await DisconnectAsync();
                            return;
                        }

                        bytesRead += read;
                    }

                    int lengthMessage = BitConverter.ToInt32(lengthBuffer, 0);
                    if (lengthMessage <= 0 || lengthMessage > _messageSize)
                    {
                        Debug.LogError($"非法消息长度: {lengthMessage} (最大允许: {_messageSize})，断开连接");
                        await DisconnectAsync();
                        return;
                    }

                    bytesRead = 0;
                    byte[] messageBuffer = new byte[lengthMessage];
                    while (bytesRead < lengthMessage)
                    {
                        int read = await _stream.ReadAsync(messageBuffer, bytesRead, lengthMessage - bytesRead);
                        if (read == 0)
                        {
                            Debug.LogWarning("读取消息体时连接中断，对端可能非正常关闭");
                            await DisconnectAsync();
                            return;
                        }

                        bytesRead += read;
                    }

                    string message = Encoding.UTF8.GetString(messageBuffer, 0, lengthMessage);
                    _messageQueue.Enqueue(message);
                    Debug.Log($"{nameof(ReceiveLoopAsync)}收到消息{message}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"{nameof(ReceiveLoopAsync)}循环接收消息异常_{e.Message}");
                    await DisconnectAsync();
                    break;
                }
            }
        }

        public async Task SendAsync(string message)
        {
            try
            {
                byte[] payload      = Encoding.UTF8.GetBytes(message);
                byte[] lengthPrefix = BitConverter.GetBytes(payload.Length);
                await _stream.WriteAsync(lengthPrefix , 0, lengthPrefix .Length);
                await _stream.WriteAsync(payload, 0, payload.Length);
                await _stream.FlushAsync();
                Debug.Log($"{nameof(SendAsync)}发送消息成功{message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(SendAsync)}发送消息异常{message}_{e.Message}");
            }
        }
    }
}