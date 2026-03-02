using AutoMapper;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Business.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDTO>> GetAllByUserAsync(int userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
    }

    public async Task<TaskDTO> GetByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);

        if (task is null)
        {
            throw new Exception("Task not found");
        }

        return _mapper.Map<TaskDTO>(task);

    }

    public async Task<TaskDTO> CreateAsync(CreateTaskDTO createTaskDto, int userId)
    {
        var task = _mapper.Map<TaskItem>(createTaskDto);

        task.UserId = userId;
        task.Status = TaskStatus.Pending;
        task.IsCompleted = false;
        task.CreatedAt = DateTime.UtcNow;

        var createTask = await _taskRepository.CreateAsync(task);

        return _mapper.Map<TaskDTO>(createTask);
    }

    public async Task<TaskDTO> UpdateAsync(int id, UpdateTaskDTO updateTaskDto, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);

        if (task is null)
        {
            throw new Exception("Task not found");
        }

        if (!string.IsNullOrWhiteSpace(updateTaskDto.Title))
            task.Title = updateTaskDto.Title;

        if (updateTaskDto.Description != null)
            task.Description = updateTaskDto.Description;

        if (updateTaskDto.DueDate.HasValue)
            task.DueDate = updateTaskDto.DueDate.Value;

        if (updateTaskDto.Status.HasValue)
            task.Status = updateTaskDto.Status.Value;

        if (updateTaskDto.Priority.HasValue)
            task.Priority = updateTaskDto.Priority.Value;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskDTO>(updatedTask);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        return await _taskRepository.DeleteAsync(id, userId);
    }

    public async Task<TaskDTO> ToggleCompleteAsync(int ind, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(ind, userId);

        if (task == null)
        {
            throw new Exception("Task not found");
        }

        task.IsCompleted = !task.IsCompleted;
        task.Status = task.IsCompleted 
            ? TaskStatus.Completed 
            : TaskStatus.Pending;
        task.CompletedAt = task.IsCompleted 
            ? DateTime.UtcNow 
            : null;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskDTO>(updatedTask);
    }
}
