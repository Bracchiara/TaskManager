using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Configuration;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.DBContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<BoardMember> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new BoardConfiguration());
        builder.ApplyConfiguration(new BoardMemberConfiguration());
        builder.ApplyConfiguration(new ColumnConfiguration());
        builder.ApplyConfiguration(new TaskItemConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
    }
    
}
