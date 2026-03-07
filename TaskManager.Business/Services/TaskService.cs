using AutoMapper;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

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

    public async Task<IEnumerable<TaskDTO>> GetAllByUserAsync(Guid userId)
    {
        var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
    }

    public async Task<TaskDTO> GetByIdAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);

        if (task is null)
        {
            throw new Exception("Task not found");
        }

        return _mapper.Map<TaskDTO>(task);

    }

    public async Task<TaskDTO> CreateAsync(CreateTaskDTO createTaskDto, Guid userId)
    {
        var task = _mapper.Map<TaskItem>(createTaskDto);

        task.Id = Guid.NewGuid();
        task.UserId = userId;
        task.CreatedAt = DateTime.UtcNow;

        var createTask = await _taskRepository.CreateAsync(task);

        return _mapper.Map<TaskDTO>(createTask);
    }

    public async Task<TaskDTO> UpdateAsync(Guid id, UpdateTaskDTO updateTaskDto, Guid userId)
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

        if (updateTaskDto.Priority.HasValue)
            task.Priority = updateTaskDto.Priority.Value;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskDTO>(updatedTask);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        return await _taskRepository.DeleteAsync(id, userId);
    }

    public async Task<TaskDTO> MoveToColumnAsync(Guid id, MoveTaskDTO moveTaskDTO, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);

        if (task is null)
        {
            throw new Exception("Task not found");
        }
        if (task.Column.BoardId != moveTaskDTO.BoardId)
        {
            throw new Exception("Task is not in this board");
        }

        task.ColumnId = moveTaskDTO.ColumnId;
        task.CompletedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateAsync(task);

        return _mapper.Map<TaskDTO>(updatedTask);
    }
}
