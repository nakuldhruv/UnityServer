using System.Text.Json;
using DockerWebApp.Models;

namespace DockerWebApp.Services;

/// <summary>
/// 将提交的邮箱记录持久化到本地 JSON 文件。
/// 文件默认存放在 ContentRoot/AppData/submissions.json，可通过配置项 Storage:DataFile 修改。
/// </summary>
public class SubmissionService
{
    private readonly string _filePath;
    private readonly object _sync = new();

    public SubmissionService(IWebHostEnvironment env, IConfiguration configuration)
    {
        var dataDirectory = AppStorage.ResolveDataDirectory(env.ContentRootPath, configuration);
        var fileName = Path.GetFileName(configuration["Storage:DataFile"] ?? "AppData/submissions.json");
        _filePath = Path.Combine(dataDirectory, fileName);
    }

    /// <summary>读取全部提交记录（按文件存储顺序返回）。</summary>
    public IReadOnlyList<Submission> GetAll()
    {
        lock (_sync)
        {
            if (!File.Exists(_filePath))
            {
                return new List<Submission>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Submission>>(json) ?? new List<Submission>();
            }
            catch (JsonException)
            {
                // 数据文件为空或损坏时，返回空列表，避免页面报错。
                return new List<Submission>();
            }
        }
    }

    /// <summary>新增一条提交记录并写入本地文件。</summary>
    public void Add(Submission submission)
    {
        lock (_sync)
        {
            var list = GetAll().ToList();
            submission.Id = list.Count == 0 ? 1 : list.Max(s => s.Id) + 1;
            submission.SubmittedAt = DateTime.Now;
            list.Add(submission);

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(list, options));
        }
    }
}
