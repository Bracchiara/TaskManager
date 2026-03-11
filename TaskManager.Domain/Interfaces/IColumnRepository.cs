using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IColumnRepository
{
    Task<Column?> GetByIdAsync(Guid id);
}
