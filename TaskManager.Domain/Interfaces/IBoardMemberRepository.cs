using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IBoardMemberRepository
{
    // Get specific member
    Task<bool> VerifyUserBoardLinked(Guid userId);
    // List all members of a board
    Task<IEnumerable<BoardMember>> GetAllByBoardIdAsync(Guid boardId);
    Task<BoardMember> CreateAsync(BoardMember boardMember);
    Task<bool> DeleteAsync(Guid id);
    // Check user role
    Task<bool> IsOwnerAsync(Guid boardId, Guid userId);

}
