using DockerWebApp.Services;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<SubmissionService>();

// 将 Data Protection 密钥持久化到本地 AppData 目录，避免应用（容器）重启后
// 防伪令牌因密钥丢失而无法解密（The key ... was not found in the key ring）。
// 若数据目录不可写则回退到默认存储方式，保证应用仍能正常启动。
var dataDirectory = AppStorage.ResolveDataDirectory(builder.Environment.ContentRootPath, builder.Configuration);
try
{
    Directory.CreateDirectory(dataDirectory);
    builder.Services
        .AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(dataDirectory));
}
catch (Exception ex)
{
    builder.Services.AddDataProtection();
    Console.WriteLine($"[DataProtection] 警告：无法在 {dataDirectory} 持久化密钥，已回退到默认方式。{ex.Message}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();