using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddAuthorization();

app.MapGet("/api/serverlist", () =>
{
    return new[]
    {
        new { id = 1, name = "一区: 盘古开天", ip = "127.0.0.1", port = 9001, status = "流畅" },
        new { id = 2, name = "二区: 女娲补天", ip = "127.0.0.1", port = 9002, status = "爆满" }
    };
});

app.MapPost("/api/login_editor", () => { });
app.MapPost("/api/login_wx", () => { });

app.Run();

// todo 接入postgres
// todo postgres配置服务器列表
// todo 提供不同平台的接口