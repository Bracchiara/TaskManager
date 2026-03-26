using TaskManager.Business.DTOs.Boards;

namespace TaskManager.Business.Interfaces;

public interface IBoardMemberService
{
    Task<BoardMemberDTO> GetByBoardAndUserAsync(Guid boardId, Guid userId);
    Task<IEnumerable<BoardMemberDTO>> GetAllByBoardIdAsync(Guid boardId);
    Task<BoardMemberDTO> CreateAsync(Guid boardId ,AddMemberDTO addMemberDTO, Guid userId);
    Task<bool> DeleteAsync(Guid boardId, Guid memberId, Guid userId);
    Task<bool> IsOwnerAsync(Guid boardId, Guid userId);
}
