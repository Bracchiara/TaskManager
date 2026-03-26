using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.Configuration;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Boards");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        // 🔗 Board -> User (Owner)
        builder.HasOne(b => b.User)
            .WithMany(u => u.Boards)
            .HasForeignKey(b => b.UserOwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔗 Board -> Columns
        builder.HasMany(b => b.Columns)
            .WithOne(c => c.Board)
            .HasForeignKey(c => c.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔗 Board -> BoardMembers
        builder.HasMany(b => b.Members)
            .WithOne(bm => bm.Board)
            .HasForeignKey(bm => bm.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}