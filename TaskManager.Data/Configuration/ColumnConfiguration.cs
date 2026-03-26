using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Data.Configuration;

public class ColumnConfiguration : IEntityTypeConfiguration<Column>
{
    public void Configure(EntityTypeBuilder<Column> builder)
    {
        builder.ToTable("Columns");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Order)
            .IsRequired();

        // 🔹 Enum
        builder.Property(c => c.Type)
            .IsRequired();

        // 🔗 Column -> Board
        builder.HasOne(c => c.Board)
            .WithMany(b => b.Columns)
            .HasForeignKey(c => c.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔗 Column -> Tasks
        builder.HasMany(c => c.Tasks)
            .WithOne(t => t.Column)
            .HasForeignKey(t => t.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 (Opcional, mas útil)
        builder.HasIndex(c => new { c.BoardId, c.Order });
    }
}
