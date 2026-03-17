using Microsoft.EntityFrameworkCore;
using TaskManager.Data.DBContext;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Data.Repositories;

public class ColumnRepository : IColumnRepository
{
    private readonly AppDbContext _context;

    public ColumnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Column?> GetByIdAsync(Guid id, Guid boardId)
    {
        return await _context.Columns
            .FirstOrDefaultAsync(c => c.Id == id && c.BoardId == boardId);
    }

    public async Task<IEnumerable<Column>> GetAllByBoardAsync(Guid boardId)
    {
        return await _context.Columns
            .Where(c => c.BoardId == boardId)
            .OrderBy(c => c.Order)
            .ToListAsync();
    }

    public async Task<Column> CreateAsync(Column column)
    {
        await _context.Columns.AddAsync(column);
        await _context.SaveChangesAsync();
        return column;
    }

    public async Task<Column> UpdateAsync(Column column)
    {
        _context.Columns.Update(column);
        await _context.SaveChangesAsync();
        return column;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid boardId)
    {
        var column = await GetByIdAsync(id, boardId);

        if (column is null)
            return false;

        _context.Columns.Remove(column);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> CountByBoardIdAsync(Guid boardId)
    {
        return await _context.Columns.CountAsync(c => c.BoardId == boardId);
    }

    public async Task<bool> NameExistsInBoardAsync(string name, Guid boardId)
    {
        return await _context.Columns
            .AnyAsync(c => c.Name == name && c.BoardId == boardId);
    }

    public async Task<int> GetNextOrderAsync(Guid boardId)
    {
        var maxOrder = await _context.Columns
            .Where(c => c.BoardId == boardId)
            .MaxAsync(c => (int?)c.Order) ?? 0;

        return maxOrder + 1;
    }

    public async Task<int> CountTasksInColumnAsync(Guid columnId)
    {
        return await _context.Tasks.CountAsync(t => t.ColumnId == columnId);
    }
}
