using MagicOnion.Client;
using Game.Shared.Services;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("http://localhost:5018");
var client = MagicOnionClient.Create<IUserService>(channel);

try
{
    Console.WriteLine("输入用户名。");
    var username = Console.ReadLine();
    Console.WriteLine("输入密码。");
    var password = Console.ReadLine();
    var user = await client.LoginAsync(username, password);
    Console.WriteLine($"✅ 登录成功！欢迎回来，{user.Name} (ID: {user.Id})");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ 登录失败: {ex.Message}");
}

Console.WriteLine("按任意键退出...");
Console.ReadKey();