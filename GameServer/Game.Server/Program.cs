using MagicOnion;
using MagicOnion.Server;

var builder = WebApplication.CreateBuilder(args);

// 1. 将 MagicOnion 服务添加到依赖注入容器
builder.Services.AddMagicOnion();

var app = builder.Build();

// 2. 将 MagicOnion 服务映射到路由
app.MapMagicOnionService();

app.Run();

/*Game.Shared  ClassLibrary	MagicOnion.Abstractions
Game.Server    Web Empty	MagicOnion.Server
Game.Client    Console	    MagicOnion.Client Grpc.Net.Client*/

