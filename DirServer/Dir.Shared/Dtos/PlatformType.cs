namespace Dir.Shared.Dtos;

public enum PlatformType
{
    UnityEditor,  // 明确是 Unity 编辑器
    Ios,
    Android,
    Wechat,
    Pc,           // PC 端（Windows/Mac/Linux）
    Web,          // 网页端（H5）
    Console       // 主机（PS/Xbox/Switch）
}