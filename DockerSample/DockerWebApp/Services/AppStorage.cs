namespace DockerWebApp.Services;

/// <summary>
/// 解析本地数据存储目录（AppData），供提交记录（SubmissionService）和
/// Data Protection 密钥共用，确保应用重启后数据与密钥都稳定可访问。
/// </summary>
public static class AppStorage
{
    /// <summary>
    /// 根据配置项 Storage:DataFile 解析数据文件所在目录（返回绝对路径）。
    /// </summary>
    public static string ResolveDataDirectory(string contentRootPath, IConfiguration configuration)
    {
        var configuredPath = configuration["Storage:DataFile"] ?? "AppData/submissions.json";
        var fullPath = Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(contentRootPath, configuredPath);
        return Path.GetDirectoryName(fullPath) ?? Path.Combine(contentRootPath, "AppData");
    }
}
