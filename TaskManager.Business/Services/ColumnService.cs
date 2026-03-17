using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Business.DTOs.Columns;
using TaskManager.Business.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Business.Services;

public class ColumnService : IColumnService
{
    private readonly IColumnRepository _columnRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;

    public ColumnService(IColumnRepository columnRepository, IBoardRepository boardRepository, IMapper mapper)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ColumnDTO>> GetAllByBoardAsync(Guid boardId)
    {
        var columns = await _columnRepository.GetAllByBoardAsync(boardId);
        return _mapper.Map<IEnumerable<ColumnDTO>>(columns);
    }

    public async Task<ColumnDTO> GetByIdAsync(Guid id, Guid boardId)
    {
        var column = await _columnRepository.GetByIdAsync(id, boardId);

        if (column is null)
            throw new Exception("Column not found.");

        return _mapper.Map<ColumnDTO>(column);
    }

    public async Task<ColumnDTO> CreateAsync(CreateColumnDTO createColumnDto, Guid boardId, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, userId);
        if (board is null)
            throw new Exception("Board not found.");

        var columnCount = await _columnRepository.CountByBoardIdAsync(boardId);
        if (columnCount > 24)
            throw new Exception("Maximum of 25 columns per board reached");

        if (await _columnRepository.NameExistsInBoardAsync(createColumnDto.Name, boardId))
            throw new Exception("Column name already exists in this board");

        var column = _mapper.Map<Column>(createColumnDto);
        column.Id = Guid.NewGuid();
        column.BoardId = boardId;
        column.Order = await _columnRepository.GetNextOrderAsync(boardId);
        column.IsDefault = false;

        var createColumn = await _columnRepository.CreateAsync(column);

        return _mapper.Map<ColumnDTO>(createColumn);
    }

    public async Task<ColumnDTO> UpdateAsync(Guid id, UpdateColumnDTO updateColumnDTO, Guid boardId, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, userId);
        if (board is null)
            throw new Exception("Board not found.");

        var column = await _columnRepository.GetByIdAsync(id, boardId);
        if (column is null)
            throw new Exception("Column not found.");

        if (column.IsDefault)
            throw new Exception("Cannot edit default columns");


        if(!string.IsNullOrWhiteSpace(updateColumnDTO.Name) && updateColumnDTO.Name != column.Name)
        {
            if (await _columnRepository.NameExistsInBoardAsync(updateColumnDTO.Name, boardId))
                throw new Exception("Column name already exists in this board");

            column.Name = updateColumnDTO.Name;
        }

        var updatedColumn = await _columnRepository.UpdateAsync(column);

        return _mapper.Map<ColumnDTO>(updatedColumn);
    }
    
    public async Task<bool> DeleteAsync(Guid id, Guid boardId, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, userId);
        if (board is null)
            throw new Exception("Board not found.");

        var column = await _columnRepository.GetByIdAsync(id, boardId);
        if (column is null)
            throw new Exception("Column not found.");

        if (column.IsDefault)
            throw new Exception("Cannot delete default columns.");

        var taskCount = await _columnRepository.CountTasksInColumnAsync(id);
        if (taskCount > 0)
            throw new Exception($"Cannot delete column with {taskCount} tasks. Move or delete tasks first.");

        return await _columnRepository.DeleteAsync(id, boardId);
    }

    public async Task<ColumnDTO> ReorderColumnAsync(Guid id, ReorderColumnDTO moveColumnDTO, Guid boardId, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(id, boardId);
        if (board is null)
            throw new Exception("Board not found.");

        var column = await _columnRepository.GetByIdAsync(id, boardId);
        if (column is null)
            throw new Exception("Column not found.");

        var columnCount = await _columnRepository.CountByBoardIdAsync(boardId);
        if (moveColumnDTO.NewOrder < 1 || moveColumnDTO.NewOrder > columnCount)
            throw new Exception($"Order must be between 1 and {columnCount}");

        column.Order = moveColumnDTO.NewOrder;

        var updatedColumn = await _columnRepository.UpdateAsync(column);

        return _mapper.Map<ColumnDTO>(updatedColumn);
    }

}
