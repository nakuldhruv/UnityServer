/* ============================================================
   项目结构说明 (Project Structure)
   ============================================================ */

/*
   Game.Shared  - 类库 (ClassLibrary)
   ├── 作用: 存放服务接口、数据传输对象(DTO)等，供服务端和客户端共享
   ├── 依赖: MagicOnion.Abstractions
   │   └── 提供 IService<T>、UnaryResult<T> 等核心接口定义
   └── 引用关系: 被 Game.Server 和 Game.Client 同时引用

   Game.Server  - ASP.NET Core Web 空项目 (Web Empty)
   ├── 作用: 实现服务端逻辑，处理客户端请求
   ├── 依赖: MagicOnion.Server
   │   └── 提供 ServiceBase<T>、服务路由、请求处理管道等
   └── 启动方式: 作为 gRPC 服务器运行，监听 HTTP/2 请求

   Game.Client  - 控制台应用 (Console)
   ├── 作用: 模拟客户端调用服务器服务，用于测试
   ├── 依赖: 
   │   ├── MagicOnion.Client  -> 提供客户端代理生成 (MagicOnionClient.Create<T>)
   │   └── Grpc.Net.Client    -> 提供 gRPC 底层通信通道 (GrpcChannel)
   └── 说明: 通过引用 Game.Shared 复用服务接口定义，确保类型安全
*/


/* ============================================================
   数据库相关包 (Database Packages - 仅需在 Game.Server 中安装)
   ============================================================ */

/*
   以下包全部安装到 Game.Server 项目中，用于集成 PostgreSQL 数据库:

   1. Npgsql.EntityFrameworkCore.PostgreSQL
      └── PostgreSQL 的 EF Core 数据库提供程序，让 EF Core 能操作 PostgreSQL

   2. Microsoft.EntityFrameworkCore.Design
      └── 提供设计时支持，用于执行迁移命令 (dotnet ef migrations add)
      └── 仅在开发时使用，运行时不需要

   3. Microsoft.EntityFrameworkCore.Tools
      └── EF Core 命令行工具的辅助包，与 Design 包配合使用
      └── 用于在包管理控制台或命令行中执行迁移操作
*/


/* ============================================================
   EF Core 全局工具安装 (Global Tool Installation)
   ============================================================ */

/*
   注意: 上面第 2、3 个包是项目内的依赖包，但执行迁移命令本身
   需要一个全局的 .NET 工具: dotnet-ef

   安装命令:
   dotnet tool install --global dotnet-ef
   
   // 新增
   dotnet ef migrations add InitialCreate
   // 改名
   dotnet ef migrations add AddUserTable
   // 新增
   dotnet ef migrations add AddUserIdToUserTable
   // 更新到数据库
   dotnet ef database update

   作用: 让你能在任何项目目录下使用 dotnet ef 命令
   例如: dotnet ef migrations add InitialCreate
        dotnet ef database update

   如果已安装旧版本，可使用 update 命令升级:
   dotnet tool update --global dotnet-ef
*/