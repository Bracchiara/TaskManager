using TaskManager.Business.DTOs.Tasks;

namespace TaskManager.Business.Interfaces;

public interface ITaskService
{
    Task<TaskDTO> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<TaskDTO>> GetAllByUserAsync(Guid userId);
    Task<TaskDTO> CreateAsync(CreateTaskDTO createTaskDto, Guid userId);
    Task<TaskDTO> UpdateAsync(Guid id, UpdateTaskDTO updateTaskDto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
