namespace TaskManager.Domain.Entities;

public class Board
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK
    public Guid UserOwnerId { get; set; }

    // 1:1
    public User User { get; set; } = null!;
    // 1:N
    public ICollection<Column> Columns { get; set; } = new List<Column>();
    public ICollection<BoardMember> Members { get; set; } = new List<BoardMember>();
}
