using Microsoft.AspNetCore.SignalR;

namespace GameServer.SignalR.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendPosition(float x, float y, float z)
        {
            string clientId = Context.ConnectionId;
            await Clients.All.SendAsync("ReceivePosition", clientId, x, y, z);
            Console.WriteLine($"玩家{clientId}：移动到X：{x}, Y：{y}, Z：{z}");
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"客户端已连接: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
    }
}
