using Game.Shared.Dtos;
using Game.Shared.Services;
using Grpc.Net.Client;
using MagicOnion.Client;

namespace Game.Client.Services;

public class UserService
{
    private readonly IUserService _userService;

    public UserService(GrpcChannel channel)
    {
        _userService = MagicOnionClient.Create<IUserService>(channel);
    }

    public async Task<ApiResponse<UserDto>> LoginAsync(string username, string password)
    {
        var response = await _userService.LoginAsync(username, password);
        return response;
    }

    public async Task<ApiResponse<UserDto>> RenameAsync(long userId, string newName)
    {
        var response = await _userService.RenameAsync(userId, newName);
        return response;
    }
    
    public async Task<ApiResponse<UserDto>> ChangePasswordAsync(long userId, string oldPassword, string newPassword)
    {
        var response = await _userService.ChangePasswordAsync(userId, oldPassword, newPassword);
        return response;
    }
}