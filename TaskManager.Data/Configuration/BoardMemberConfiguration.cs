using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.Configuration;

public class BoardMemberConfiguration : IEntityTypeConfiguration<BoardMember>
{
    public void Configure(EntityTypeBuilder<BoardMember> builder)
    {
        builder.ToTable("BoardMembers");

        // 🔑 PK composta
        builder.HasKey(bm => new { bm.BoardId, bm.UserId });

        // 🔗 BoardMember -> Board
        builder.HasOne(bm => bm.Board)
            .WithMany(b => b.Members)
            .HasForeignKey(bm => bm.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔗 BoardMember -> User
        builder.HasOne(bm => bm.User)
            .WithMany(u => u.BoardMembers)
            .HasForeignKey(bm => bm.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // 📌 Role
        builder.Property(bm => bm.Role)
            .IsRequired()
            .HasMaxLength(50);
    }
}