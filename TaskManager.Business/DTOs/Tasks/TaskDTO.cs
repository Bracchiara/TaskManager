using TaskManager.Domain.Enums;


namespace TaskManager.Business.DTOs.Tasks;

public class TaskDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompleteAt { get; set; }
    public Guid ColumnId { get; set; }

}
