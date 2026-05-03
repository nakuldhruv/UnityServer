using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace SignalR
{
    public class SignalRManager : MonoBehaviour
    {
        private HubConnection _connection;
        private string _url = "http://127.0.0.1:5115/gamehub";

        private async void Start()
        {
            _connection = new HubConnectionBuilder().WithUrl(_url).WithAutomaticReconnect().Build();
            _connection.On<string, float, float, float>("ReceivePosition", ReceivePosition);

            try
            {
                await _connection.StartAsync();
                Debug.Log("Connected");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to connect");
            }
        }

        private async void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                await SendPosition(transform.position);
            }
        }

        public async Task SendPosition(Vector3 position)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendPosition", position.x, position.y, position.z);   
            }
        }

        public async Task ReceivePosition(string clientId, float x, float y, float z)
        {
            Debug.Log($"玩家{clientId}：移动到X：{x}, Y：{y}, Z：{z}");
        }

        private async void OnDestroy()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
            }
        }
    }    
}

