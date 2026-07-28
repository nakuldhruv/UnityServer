using MessagePack;

namespace Game.Shared.Dtos
{
    [MessagePackObject]
    public class UserDto
    {
        [Key(0)]
        public long Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Password { get; set; }  // 演示用，生产环境不应明文存储密码
    }
}