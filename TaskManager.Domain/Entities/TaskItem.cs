using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public DateTime? StartedAt { get; set; }

    public Guid? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

    // FK
    public Guid ColumnId { get; set; }

    // 1:1
    public Column Column { get; set; } = null!;
}
