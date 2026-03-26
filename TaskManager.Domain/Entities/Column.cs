using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class Column
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public ColumnType Type { get; set; }

    // FK
    public Guid BoardId { get; set; }

    // 1:1
    public Board Board { get; set; } = null!;
    // 1:N
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

}
