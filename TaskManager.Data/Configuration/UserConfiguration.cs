using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        // 🔗 User -> Boards (owner)
        builder.HasMany(u => u.Boards)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserOwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔗 User -> BoardMembers (participação)
        builder.HasMany(u => u.BoardMembers)
            .WithOne(bm => bm.User)
            .HasForeignKey(bm => bm.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
