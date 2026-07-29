namespace DirServer.Entities;

public class ServerEntity
{
    public int Id { get; set; }
    public string Name { get; set; }          // 如 "一区: 盘古开天"
    public string Ip { get; set; }            // 如 "127.0.0.1"
    public int Port { get; set; }             // 如 9001
    public string Status { get; set; }        // 如 "流畅"、"爆满"
}