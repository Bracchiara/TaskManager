using TaskManager.Domain.Enums;

namespace TaskManager.Business.DTOs.Boards;

public class AddMemberDTO
{
    public string Email { get; set; } = string.Empty;
    public BoardRole Role { get; set; } = BoardRole.Member;

}