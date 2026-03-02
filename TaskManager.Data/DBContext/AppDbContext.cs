using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.DBContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Relacionamento User -> Tasks
        mb.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Email único
        mb.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Title obrigatório
        mb.Entity<TaskItem>()
            .Property(t =>t.Title)
            .IsRequired()
            .HasMaxLength(200);
    }
}
