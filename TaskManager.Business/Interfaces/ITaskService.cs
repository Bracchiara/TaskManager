using TaskManager.Business.DTOs.Tasks;

namespace TaskManager.Business.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDTO>> GetAllByUserAsync(int userId);
    Task<TaskDTO> GetByIdAsync(int id, int userId);
    Task<TaskDTO> CreateAsync(CreateTaskDTO createTaskDto, int userId);
    Task<TaskDTO> UpdateAsync(int id, UpdateTaskDTO updateTaskDto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
    Task<TaskDTO> ToggleCompleteAsync(int ind, int userId);
}
