using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id, int userId);
}
