var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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