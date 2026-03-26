using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Business.DTOs.Boards;
using TaskManager.Business.DTOs.Columns;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.Interfaces;
using TaskManager.Business.Services;

namespace TaskManager.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;
    private readonly IColumnService _columnService;
    private readonly IBoardMemberService _boardMemberService;

    public BoardController(IBoardService boardService, IColumnService columnService, IBoardMemberService boardMemberService)
    {
        _boardService = boardService;
        _columnService = columnService;
        _boardMemberService = boardMemberService;
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
    public async Task<ActionResult<BoardDTO>> CreateBoard([FromBody] CreateBoardDTO createBoardDTO)
    {
        var userId = GetUserId();
        var board = await _boardService.CreateAsync(createBoardDTO, userId);

        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    // Update a existing board
    [HttpPut("{id}")]
    public async Task<ActionResult<BoardDTO>> UpdateBoard(Guid id, [FromBody] UpdateBoardDTO updateBoardDTO)
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
    [HttpDelete("{id}")]
    public async Task<ActionResult<BoardDTO>> DeleteBoard(Guid id)
    {
        var userId = GetUserId();
        var result = await _boardService.DeleteAsync(id, userId);

        if (!result)
            return NotFound(new {message = "Board not found"});

        return NoContent();
    }

    // List all columns from a board
    [HttpGet("{boardId}/columns")]
    public async Task<ActionResult<IEnumerable<ColumnDTO>>> GetColumns(Guid boardId)
    {
        var columns = await _columnService.GetAllByBoardAsync(boardId);
        return Ok(columns);
    }

    // Create column
    [HttpPost("{boardId}/columns")]
    public async Task<ActionResult<ColumnDTO>> CreateColumn(Guid boardId, [FromBody] CreateColumnDTO createColumnDTO)
    {
        try
        {
            var userId = GetUserId();
            var column = await _columnService.CreateAsync(createColumnDTO, boardId, userId);

            return Ok(column);
        }
        catch(Exception ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }

    // Edit column name
    [HttpPut("{boardId}/columns/{id}")]
    public async Task<ActionResult<ColumnDTO>> UpdateColumn(Guid boardId, Guid id, [FromBody] UpdateColumnDTO updateColumnDTO)
    {
        try
        {
            var userId = GetUserId();
            var column = await _columnService.UpdateAsync(id, updateColumnDTO, boardId, userId);

            return Ok(column);
        }
        catch (Exception ex)
        {
            return NotFound(new {message = ex.Message});
        }
    }

    // Delete column
    [HttpDelete("{boardId}/columns/{id}")]
    public async Task<ActionResult<ColumnDTO>> DeleteColumn(Guid boardId, Guid id)
    {
        try
        { 
            var userId = GetUserId();
            var column = await _columnService.DeleteAsync(id, boardId, userId);

            if (!column)
                return NotFound(new { message = "Column not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message});
        }
    }

    // Move column
    [HttpPatch("{boardId}/columns/{id}/reorder")]
    public async Task<ActionResult<ColumnDTO>> ReorderColumn(Guid boardId, Guid id, [FromBody] ReorderColumnDTO reorderColumnDTO)
    {
        try
        {
            var userId = GetUserId();
            var column = await _columnService.ReorderColumnAsync(id, reorderColumnDTO, boardId, userId);
            return Ok(column);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Add member
    [HttpPost("{boardId}/members")]
    public async Task<ActionResult<BoardMemberDTO>> AddMember(Guid boardId, [FromBody] AddMemberDTO addMemberDTO)
    {
        try
        {
            var userId = GetUserId();
            var member = await _boardMemberService.CreateAsync(boardId, addMemberDTO, userId);
            return Ok(member);
        }
        catch(Exception ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }

    // List members
    [HttpGet("{boardId}/members")]
    public async Task<ActionResult<BoardMemberDTO>> GetMembers(Guid boardId)
    {
        var members = await _boardMemberService.GetAllByBoardIdAsync(boardId);
        return Ok(members);
    }

    // Remove member
    [HttpDelete("{boardId}/members/{memberId}")]
    public async Task<ActionResult<BoardMemberDTO>> DeleteMember(Guid boardId, Guid memberId)
    {
        try
        {
            var userId = GetUserId();
            var member = await _boardMemberService.DeleteAsync(boardId, memberId, userId);

            if (!member)
                return NotFound(new { message = "Member not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message});
        }
    }
}
