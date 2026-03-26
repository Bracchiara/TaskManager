using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // Get User ID from JWT token
    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? "0");
    }

    // List all tasks for the authenticated user
    [HttpGet]
    public async Task<ActionResult> GetTasks()
    {
        var userId = GetUserId();
        var tasks = await _taskService.GetAllByUserAsync(userId);
        return Ok(tasks);
    }

    // Get a specific task by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDTO>> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.GetByIdAsync(id, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return NotFound(new {message = ex.Message });
        }
    }

    // Create a new task
    [HttpPost]
    public async Task<ActionResult<TaskDTO>> Create([FromBody] CreateTaskDTO createTaskDTO)
    {
        var userId = GetUserId();
        var task = await _taskService.CreateAsync(createTaskDTO, userId);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    // Update an existing task
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDTO>> Update(Guid id, [FromBody] UpdateTaskDTO updateTaskDTO)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.UpdateAsync(id, updateTaskDTO, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Delete a task
    [HttpDelete("{id}")]
    public async Task<ActionResult<TaskDTO>> Delete(Guid id)
    {
        var userId = GetUserId();
        var result = await _taskService.DeleteAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Task not found"});

        return NoContent();
    }

    [HttpPatch("{id}/move")]
    public async Task<ActionResult<TaskDTO>> MoveToColumn(Guid id, [FromBody] MoveTaskDTO moveTaskDTO)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.MoveToColumnAsync(id, moveTaskDTO, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/assign")]
    public async Task<ActionResult<TaskDTO>> AssignTask(Guid id, [FromBody] AssignTaskDTO assignTaskDTO)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.AssignTaskAsync(id, assignTaskDTO, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }
}
