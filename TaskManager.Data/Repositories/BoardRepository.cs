using Microsoft.EntityFrameworkCore;
using TaskManager.Data.DBContext;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Data.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;

    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Board?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Boards
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }

    public async Task<IEnumerable<Board>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Boards
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<Board> CreateAsync(Board board)
    {
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();
        return board;
    }

    public async Task<Board> UpdateAsync(Board board)
    {
        _context.Boards.Update(board);
        await _context.SaveChangesAsync();
        return board;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var board = await GetByIdAsync(id, userId);
        if (board == null) return false;

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();
        return true;
    }

}
