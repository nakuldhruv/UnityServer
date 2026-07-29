namespace Dir.Shared.Dtos;

public class LoginRequest
{
    public PlatformType Platform { get; set; }  // "wx", "dy", "ks", "pc", "adr", "ios"
    public string OpenId { get; set; }    // 平台返回的用户唯一标识
    public string Token { get; set; }     // 平台的 access_token
    public string? DeviceId { get; set; }  // 设备指纹（可选）
    public string? Nickname { get; set; }  // 昵称（可选，首次注册用）
}