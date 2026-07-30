using Game.Shared.Dtos;
using MagicOnion;

namespace Game.Shared.Services;

public interface IUserService : IService<IUserService>
{
    UnaryResult<ApiResponse<UserDto>> LoginAsync(string username, string password);

    UnaryResult<ApiResponse<UserDto>> RenameAsync(long userId, string newName);
    
    UnaryResult<ApiResponse<UserDto>> ChangePasswordAsync(long userId, string oldPassword, string newPassword);
}