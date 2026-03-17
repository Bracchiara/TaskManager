using AutoMapper;
using TaskManager.Business.DTOs.Boards;
using TaskManager.Business.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Business.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;

    public BoardService(IBoardRepository boardRepository, IMapper mapper)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;
    }

    public async Task<BoardDTO> GetByIdAsync(Guid id, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(id, userId);

        if (board == null)
        {
            throw new Exception("Board not found.");
        }

        return _mapper.Map<BoardDTO>(board);
    }

    public async Task<IEnumerable<BoardDTO>> GetAllByUserAsync(Guid userId)
    {
        var boards = await _boardRepository.GetAllByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<BoardDTO>>(boards);
    }

    public async Task<BoardDTO> CreateAsync(CreateBoardDTO createBoardDto, Guid userId)
    {
        var board = _mapper.Map<Board>(createBoardDto);

        board.Id = Guid.NewGuid();
        board.UserId = userId;
        board.CreatedAt = DateTime.UtcNow;
        board.Columns = new List<Column>
        {
            new Column { Id = Guid.NewGuid(), Name = "Backlog", Order = 1, BoardId = board.Id, IsDone = false, IsDefault = true},
            new Column { Id = Guid.NewGuid(), Name = "To Do", Order = 2, BoardId = board.Id, IsDone = false, IsDefault = true},
            new Column { Id = Guid.NewGuid(), Name = "In Progress", Order = 3, BoardId = board.Id, IsDone = false, IsDefault = true},
            new Column { Id = Guid.NewGuid(), Name = "Review", Order = 4, BoardId = board.Id, IsDone = false, IsDefault = true},
            new Column { Id = Guid.NewGuid(), Name = "Done", Order = 5, BoardId = board.Id, IsDone = true, IsDefault = true},
        };


        var createBoard = await _boardRepository.CreateAsync(board);

        return _mapper.Map<BoardDTO>(createBoard);
    }

    public async Task<BoardDTO> UpdateAsync(Guid id, UpdateBoardDTO updateBoardDto, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(id, userId);

        if (board == null)
        {
            throw new Exception("Board not found.");
        }

        if (!string.IsNullOrWhiteSpace(updateBoardDto.Name))
            board.Name = updateBoardDto.Name;

        if (updateBoardDto.Description != null)
            board.Description = updateBoardDto.Description;

        var updatedBoard = await _boardRepository.UpdateAsync(board);
        return _mapper.Map<BoardDTO>(updatedBoard);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        return await _boardRepository.DeleteAsync(id, userId);
    }
}
