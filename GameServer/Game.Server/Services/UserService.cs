using Game.Server.Data;
using Game.Server.Entities;
using MagicOnion;
using MagicOnion.Server;
using Game.Shared.Dtos;
using Game.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Game.Server.Services
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async UnaryResult<ApiResponse<UserDto>> LoginAsync(string username, string password)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == username);
            long maxUserId = await _dbContext.Users.MaxAsync(u => (long?)u.UserId) ?? 100000;
            if (userEntity == null)
            {
                long newUserId = ++maxUserId;
                userEntity = new UserEntity
                {
                    UserId = newUserId,
                    Name = username,
                    Password = password,
                };
                // 添加到数据库上下文
                await _dbContext.Users.AddAsync(userEntity);
                // 保存到数据库（真正写入）
                await _dbContext.SaveChangesAsync();
                // 返回注册后的用户信息（不含密码）
                return new ApiResponse<UserDto>()
                {
                    Success = true,
                    Message = $"新用户注册成功: {username}",
                    Data = new UserDto()
                    {
                        UserId = newUserId,
                        Name = userEntity.Name,
                    }
                };
            }
            else
            {
                if (userEntity.Password == password)
                {
                    return new ApiResponse<UserDto>()
                    {
                        Success = userEntity.Password == password,
                        Message = $"用户登录成功: {username}",
                        Data = new UserDto()
                        {
                            UserId = maxUserId,
                            Name = userEntity.Name,
                        }
                    };
                }
                else
                {
                    return new ApiResponse<UserDto>()
                    {
                        Success = false,
                        Message = "密码错误！",
                    };
                }
            }
        }

        public async UnaryResult<ApiResponse<UserDto>> RenameAsync(long userId, string newName)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userEntity == null)
            {
                return new ApiResponse<UserDto>()
                {
                    Success = false,
                    Message = $"用户不存在：{userId}"
                };
            }
            else
            {
                userEntity.Name = newName;
                await _dbContext.SaveChangesAsync();
                return new ApiResponse<UserDto>()
                {
                    Success = true,
                    Message = "修改名称成功。",
                    Data = new UserDto()
                    {
                        UserId = userId,
                        Name = userEntity.Name,
                    }
                };
            }
        }

        public async UnaryResult<ApiResponse<UserDto>> ChangePasswordAsync(long userId, string oldPassword, string newPassword)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userEntity == null)
            {
                return new ApiResponse<UserDto>()
                {
                    Success = false,
                    Message = $"用户不存在：{userId}"
                };
            }
            else
            {
                if (oldPassword != userEntity.Password)
                {
                    return new ApiResponse<UserDto>()
                    {
                        Success = false,
                        Message = $"密码错误：{userId}"
                    };
                }

                userEntity.Password = newPassword;
                await _dbContext.SaveChangesAsync();
                return new ApiResponse<UserDto>()
                {
                    Success = true,
                    Message = "密码修改成功。",
                    Data = new UserDto()
                    {
                        UserId = userId,
                        Name = userEntity.Name,
                    }
                };
            }
        }
    }
}