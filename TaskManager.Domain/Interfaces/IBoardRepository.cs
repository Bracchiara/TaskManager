using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(Guid id);
    Task<IEnumerable<Board>> GetAllByUserIdAsync(Guid userId);
    Task<Board> CreateAsync(Board board);
    Task<Board> UpdateAsync(Board board);
    Task<bool> DeleteAsync(Guid id);
}
