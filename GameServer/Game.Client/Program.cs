using MagicOnion.Client;
using Game.Shared;
using Game.Shared.Services;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:5001"); // 改为你的服务器地址
var client = MagicOnionClient.Create<IUserService>(channel);

// 创建用户
var newUser = await client.CreateUserAsync("Alice");
Console.WriteLine($"创建成功: Id={newUser.Id}, Name={newUser.Name}");

// 获取用户
var user = await client.GetUserAsync(newUser.Id);
Console.WriteLine($"获取用户: Id={user.Id}, Name={user.Name}");