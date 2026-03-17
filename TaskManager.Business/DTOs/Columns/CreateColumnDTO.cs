using System.ComponentModel.DataAnnotations;

namespace TaskManager.Business.DTOs.Columns;

public class CreateColumnDTO
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
