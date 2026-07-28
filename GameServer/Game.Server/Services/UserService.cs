using MagicOnion;
using MagicOnion.Server;
using Game.Shared;
using System.Collections.Concurrent;
using Game.Shared.Dtos;
using Game.Shared.Services;

namespace Game.Server.Services
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        private static readonly ConcurrentDictionary<int, UserDto> _users = new();
        private static int _nextId = 1;

        public async UnaryResult<UserDto> GetUserAsync(int id)
        {
            // 模拟异步（实际可省略）
            await Task.CompletedTask;
            _users.TryGetValue(id, out var user);
            return user;
        }

        public async UnaryResult<UserDto> CreateUserAsync(string name)
        {
            await Task.CompletedTask;
            var user = new UserDto { Id = _nextId++, Name = name };
            _users.TryAdd(user.Id, user);
            return user;
        }
    }
}