namespace Dir.Shared.Dtos;

public class LoginResult
{
    public int UserId { get; set; }
    public string SessionToken { get; set; }
    public string ServerIp { get; set; }
    public int ServerPort { get; set; }
    public PlatformType Platform { get; set; }
    public string NickName { get; set; }
}