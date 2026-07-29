using System.Text;
using DirServer.Data;
using DirServer.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // 自动创建表（如果迁移未应用）
    await db.Database.MigrateAsync();

    // 如果表里没有数据，则插入测试数据
    if (!await db.Servers.AnyAsync())
    {
        var servers = new List<ServerEntity>
        {
            new() { Name = "一区: 盘古开天", Ip = "127.0.0.1", Port = 9001, Status = "流畅" },
            new() { Name = "二区: 女娲补天", Ip = "127.0.0.1", Port = 9002, Status = "爆满" },
            new() { Name = "三区: 后羿射日", Ip = "127.0.0.1", Port = 9003, Status = "流畅" },
            new() { Name = "四区: 精卫填海", Ip = "127.0.0.1", Port = 9004, Status = "维护" },
            new() { Name = "五区: 夸父逐日", Ip = "127.0.0.1", Port = 9005, Status = "流畅" }
        };
        await db.Servers.AddRangeAsync(servers);
        await db.SaveChangesAsync();
        Console.WriteLine("✅ 测试服务器数据已初始化。");
    }
}

app.MapGet("/api/serverlist", async (AppDbContext db) =>
{
    var servers = await db.Servers.ToListAsync();
    return servers.Select(s => new
    {
        id = s.Id,
        name = s.Name,
        ip = s.Ip,
        port = s.Port,
        status = s.Status
    });
});

app.MapPost("/api/login_editor", () => { });
app.MapPost("/api/login_wx", () => { });

app.Run();

// todo 接入postgres
// todo postgres配置服务器列表
// todo 提供不同平台的接口

/*dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate
dotnet ef database update
*/