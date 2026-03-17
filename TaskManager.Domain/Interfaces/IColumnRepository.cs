using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IColumnRepository
{
    Task<Column?> GetByIdAsync(Guid id, Guid boardId);
    Task<IEnumerable<Column>> GetAllByBoardAsync(Guid boardId);
    Task<Column> CreateAsync(Column column);
    Task<Column> UpdateAsync(Column column);
    Task<bool> DeleteAsync(Guid id, Guid boardId);
    Task<int> CountByBoardIdAsync(Guid boardId);
    Task<bool> NameExistsInBoardAsync(string name, Guid boardId);
    Task<int> GetNextOrderAsync(Guid boardId);
    Task<int> CountTasksInColumnAsync(Guid columnId);
}
