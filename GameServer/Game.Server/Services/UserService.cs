using Game.Server.Data;
using Game.Server.Entities;        // 新增：引用 UserEntity
using MagicOnion;
using MagicOnion.Server;
using Game.Shared.Dtos;
using Game.Shared.Services;
using Microsoft.EntityFrameworkCore;  // 新增：用于异步查询

namespace Game.Server.Services
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⚠️ 关键改动：方法签名改为 async，返回 UnaryResult<UserDto>
        public async UnaryResult<UserDto> LoginAsync(string username, string password)
        {
            // 1. 从数据库查询用户（异步）
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Name == username);

            // 2. 如果用户不存在 → 注册新用户
            if (userEntity == null)
            {
                // 创建新用户实体
                userEntity = new UserEntity
                {
                    Name = username,
                    PasswordHash = password,  // ⚠️ 演示用明文，生产环境应存哈希值
                    CreatedAt = DateTime.UtcNow
                };

                // 添加到数据库上下文
                await _dbContext.Users.AddAsync(userEntity);
                // 保存到数据库（真正写入）
                await _dbContext.SaveChangesAsync();

                Console.WriteLine($"✅ 新用户注册成功: {username}");

                // 返回注册后的用户信息（不含密码）
                return new UserDto
                {
                    Id = userEntity.Id,
                    Name = userEntity.Name,
                    Password = null
                };
            }

            // 3. 用户存在 → 验证密码
            if (userEntity.PasswordHash != password)
            {
                throw new Exception("密码错误！");
            }

            Console.WriteLine($"✅ 用户登录成功: {username}");

            // 4. 登录成功，返回用户信息
            return new UserDto
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Password = null
            };
        }
    }
}