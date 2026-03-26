namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 1:N
    public ICollection<BoardMember> BoardMembers { get; set; } = new List<BoardMember>();
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}
