using TaskManager.Domain.Enums;

namespace TaskManager.Business.DTOs.Boards;

public class BoardMemberDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public BoardRole Role { get; set; }
    public DateTime AddedAt { get; set; }
}