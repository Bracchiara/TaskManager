using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Business.DTOs.Tasks;

public class UpdateTaskDTO
{
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string? Title { get; set; }

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
}
