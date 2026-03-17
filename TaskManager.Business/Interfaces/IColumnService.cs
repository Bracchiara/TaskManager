using TaskManager.Business.DTOs.Columns;
using TaskManager.Domain.Entities;

namespace TaskManager.Business.Interfaces;

public interface IColumnService
{
    Task<ColumnDTO> GetByIdAsync(Guid id, Guid boardId);
    Task<IEnumerable<ColumnDTO>> GetAllByBoardAsync(Guid boardId);
    Task<ColumnDTO> CreateAsync(CreateColumnDTO createColumnDTO, Guid boarId, Guid userId);
    Task<ColumnDTO> UpdateAsync(Guid id, UpdateColumnDTO updateColumnDTO, Guid boardId, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid boardId, Guid userId);
    Task<ColumnDTO> ReorderColumnAsync(Guid id, ReorderColumnDTO reorderColumnDTO, Guid boardId, Guid userId);
}
