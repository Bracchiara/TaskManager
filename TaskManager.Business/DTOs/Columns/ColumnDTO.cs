namespace TaskManager.Business.DTOs.Columns;

public class ColumnDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsDone { get; set; } = false;
    public bool IsDefault { get; set; } = false;
    public Guid BoardId { get; set; }
}
