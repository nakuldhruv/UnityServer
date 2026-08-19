var builder = WebApplication.CreateBuilder(args);

// 注册调用 Joke API 的 HttpClient
builder.Services.AddHttpClient("JokeApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["JokeApi:BaseUrl"] ?? "http://localhost:5251");
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();