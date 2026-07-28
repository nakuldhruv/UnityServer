using Game.Shared.Dtos;
using MagicOnion;

namespace Game.Shared.Services;

public interface IUserService : IService<IUserService>
{
    UnaryResult<UserDto> LoginAsync(string username, string password);
}