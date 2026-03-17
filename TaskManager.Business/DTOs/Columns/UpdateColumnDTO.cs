using System.ComponentModel.DataAnnotations;

namespace TaskManager.Business.DTOs.Columns;

public class UpdateColumnDTO
{
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { set; get; } = string.Empty;
}
