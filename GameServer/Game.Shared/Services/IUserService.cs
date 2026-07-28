using Game.Shared.Dtos;
using MagicOnion;

namespace Game.Shared.Services;

public interface IUserService : IService<IUserService>
{
    UnaryResult<UserDto> GetUserAsync(int id);
    UnaryResult<UserDto> CreateUserAsync(string name);
}