using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(Guid userId);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(Guid id);
}
