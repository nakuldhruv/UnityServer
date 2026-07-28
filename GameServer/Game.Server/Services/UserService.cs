using MagicOnion;
using MagicOnion.Server;
using Game.Shared.Dtos;
using Game.Shared.Services;

namespace Game.Server.Services
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        private long _userId;
        private List<UserDto> _allUsers = new List<UserDto>();
        private List<UserDto> _onlineUsers = new List<UserDto>();

        public UnaryResult<UserDto> LoginAsync(string username, string password)
        {
            if (!_allUsers.Exists(x => x.Name == username && x.Password == password))
            {
                // todo 注册
                Console.WriteLine("注册");
            }
            else
            {
                // 登陆
            }

            return new UnaryResult<UserDto>();
        }
    }
}