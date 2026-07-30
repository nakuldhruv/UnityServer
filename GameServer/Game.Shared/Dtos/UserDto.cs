using MessagePack;

namespace Game.Shared.Dtos
{
    [MessagePackObject]
    public class UserDto
    {
        [Key(0)]
        public long UserId { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Password { get; set; }
    }
}