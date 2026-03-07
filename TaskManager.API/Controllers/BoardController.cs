using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Business.DTOs.Boards;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    // Get User ID from JWT token
    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? "0");
    }

    // List all boards for the user
    [HttpGet]
    public async Task<ActionResult> GetBoard()
    {
        var userId = GetUserId();
        var boards = await _boardService.GetAllByUserAsync(userId);

        return Ok(boards);
    }

    // Get board by id
    [HttpGet("{id}")]
    public async Task<ActionResult<BoardDTO>> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var board = await _boardService.GetByIdAsync(id, userId);

            return Ok(board);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Create a new board
    [HttpPost]
    public async Task<ActionResult<BoardDTO>> Create([FromBody] CreateBoardDTO createBoardDTO)
    {
        var userId = GetUserId();
        var board = await _boardService.CreateAsync(createBoardDTO, userId);

        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    // Update a existing board
    [HttpPost("{id}")]
    public async Task<ActionResult<BoardDTO>> Update(Guid id, [FromBody] UpdateBoardDTO updateBoardDTO)
    {
        try
        {
            var userId = GetUserId();
            var board = await _boardService.UpdateAsync(id, updateBoardDTO, userId);
            
            return Ok(board);
        }
        catch (Exception ex)
        {
            return NotFound(new {message = ex.Message});
        }
    }

    // Delete a board
    [HttpDelete]
    public async Task<ActionResult<BoardDTO>> Delete(Guid id)
    {
        var userId = GetUserId();
        var result = await _boardService.DeleteAsync(id, userId);

        if (!result)
            return NotFound(new {message = "Board not found"});

        return NoContent();
    }
}
