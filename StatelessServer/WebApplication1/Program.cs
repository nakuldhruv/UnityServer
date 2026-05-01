// 引入 MongoDB 官方驱动库，只有引入了它，才能使用 MongoClient、IMongoClient 等类
using MongoDB.Driver;

// 创建一个 Web 应用程序的构建器 (Builder)
// 它是整个服务器的起点，负责配置服务（Service）、日志、配置文件（appsettings.json）等
var builder = WebApplication.CreateBuilder(args);

// --- 第一部分：从配置文件读取数据 ---

// 从 appsettings.json 中读取 "PlayerDbSettings" 节点下的 "ConnectionString" 的值
// 目的：避免把数据库密码和 IP 地址硬编码在代码里，方便后期修改
var connectionString = builder.Configuration.GetSection("PlayerDbSettings:ConnectionString").Value;

// 读取数据库名称（例如 "GameServerDB"）
var databaseName = builder.Configuration.GetSection("PlayerDbSettings:DatabaseName").Value;


// --- 第二部分：依赖注入 (Dependency Injection) 注册 ---

// 将 IMongoClient 注册为“单例” (Singleton) 模式
// “单例”意味着：服务器运行期间，不管有多少玩家连进来，永远只创建一个数据库连接池实例。
// 这样可以极大地节省内存并提高性能，因为建立数据库连接是非常耗时的操作。
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

// 告诉服务器：我们要使用“控制器”模式（Controller）来处理 API 请求
// 控制器就是你写的那些 PlayerController.cs，没有这一行，服务器找不到你的接口
builder.Services.AddControllers();

// 注册 API 端点资源管理器
// 这是为了配合 Swagger 使用，它会扫描你代码里所有的 [HttpGet]、[HttpPost] 接口
builder.Services.AddEndpointsApiExplorer();

// 注册 Swagger 生成器
// 它的作用是根据你的代码自动生成一份 OpenAPI 规范的 JSON 文档（描述了接口长什么样）
builder.Services.AddSwaggerGen();


// --- 第三部分：构建并配置请求管道 (Middleware) ---

// 调用 Build() 方法，正式创建出“应用程序实例 (app)”
// 在这行之后，就不能再注册新的服务了，只能配置它是如何运行的
var app = builder.Build();

// 判断当前运行环境是否为“开发模式” (Development)
// 这是一个安全措施：在正式上线时，我们通常会关闭 Swagger 界面，防止黑客通过文档研究你的接口漏洞
if (app.Environment.IsDevelopment())
{
    // 启用 Swagger 中间件，允许服务器生成 swagger.json 供前端（或自己）查看
    app.UseSwagger();

    // 启用 Swagger 可视化 UI 界面
    // 启用后，你可以通过浏览器访问 http://localhost:端口/swagger 看到图形化测试页面
    app.UseSwaggerUI();
}

// 告诉服务器使用“路由映射”
// 当一个 HTTP 请求过来（比如 /api/player/save），它会自动找到对应的 PlayerController
app.MapControllers();

// 正式启动服务器，开始监听网络端口
// 这是一个阻塞调用，直到你手动停止服务器或程序崩溃，否则它会一直运行并处理请求
app.Run();