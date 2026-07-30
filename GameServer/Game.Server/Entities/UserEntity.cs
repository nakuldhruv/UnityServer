using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Server.Entities;

[Table("users")] 
public class UserEntity
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}