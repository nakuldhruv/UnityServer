using Microsoft.EntityFrameworkCore;
using Game.Server.Entities;

namespace Game.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 可以在这里配置表名、索引等（可选）
        modelBuilder.Entity<UserEntity>().ToTable("users");
    }
}