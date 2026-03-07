using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;

namespace TaskManager.Business.DTOs.Tasks;

public class CreateTaskDTO
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Due date is required.")]
    public DateTime DueDate { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public Guid ColumnId { get; set; }
}
