using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Server.Entities;

[Table("users")] 
public class UserEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }  // 生产环境用哈希，演示先存明文
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}