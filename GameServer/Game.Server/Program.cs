var builder = WebApplication.CreateBuilder(args);

// HTTP/1.1 是“单车道文本通信”（慢，老式），HTTP/2 是“多车道二进制通信”（快，现代）
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5018,
        listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
});

builder.Services.AddMagicOnion();

var app = builder.Build();

app.MapMagicOnionService();

app.Run();

/*Game.Shared  ClassLibrary	MagicOnion.Abstractions
Game.Server    Web Empty	MagicOnion.Server
Game.Client    Console	    MagicOnion.Client Grpc.Net.Client*/