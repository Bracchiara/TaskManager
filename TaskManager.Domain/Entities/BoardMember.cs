using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class BoardMember
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public BoardRole Role { get; set; }
    public DateTime AddedAt { get; set; }

    public Board Board { get; set; } = null!;
    public User User { get; set; } = null!;
}
