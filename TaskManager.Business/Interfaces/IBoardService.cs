using TaskManager.Business.DTOs.Boards;

namespace TaskManager.Business.Interfaces;

public interface IBoardService
{
    Task<BoardDTO> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<BoardDTO>> GetAllByUserAsync(Guid userId);
    Task<BoardDTO> CreateAsync(CreateBoardDTO createBoardDto, Guid userId);
    Task<BoardDTO> UpdateAsync(Guid id, UpdateBoardDTO updateBoardDto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
