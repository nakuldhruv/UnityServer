using Microsoft.EntityFrameworkCore;
using DirServer.Entities;

namespace DirServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ServerEntity> Servers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 指定表名（可选）
        modelBuilder.Entity<ServerEntity>().ToTable("servers");
    }
}