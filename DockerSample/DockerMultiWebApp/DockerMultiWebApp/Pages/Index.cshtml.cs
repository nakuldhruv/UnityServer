using System.Collections;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerMultiWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public string? Joke { get; set; }

    /// <summary>compose.yaml 中为 Web 应用配置的自定义环境变量</summary>
    public string? TestEnvironment => Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");

    /// <summary>ASP.NET Core 当前运行环境</summary>
    public string? AspNetCoreEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    /// <summary>容器/主机名</summary>
    public string? HostName => Environment.GetEnvironmentVariable("HOSTNAME");

    /// <summary>全部环境变量（按名称排序）</summary>
    public List<KeyValuePair<string, string>> EnvironmentVariables { get; private set; } = new();

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("JokeApi");
        try
        {
            var response = await client.GetFromJsonAsync<JokeResponse>("/joke");
            Joke = response?.Text ?? "今天没有笑话，明天再来吧。";
        }
        catch
        {
            Joke = "笑话服务暂时罢工了，请稍后再试 😅";
        }

        EnvironmentVariables = Environment.GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .Select(e => new KeyValuePair<string, string>(e.Key.ToString() ?? string.Empty, e.Value?.ToString() ?? string.Empty))
            .OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public record JokeResponse(string? Text);
}