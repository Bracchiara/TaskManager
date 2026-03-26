using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data.DBContext;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Data.Repositories;

public class BoardMemberRepository : IBoardMemberRepository
{
    private readonly AppDbContext _context;

    public BoardMemberRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<bool> VerifyUserBoardLinked(Guid userId)
    {
        var user = await _context.Members.FirstOrDefaultAsync(bm  => bm.UserId == userId);

        return user != null;
    }

    public async Task<IEnumerable<BoardMember>> GetAllByBoardIdAsync(Guid boardId)
    {
        return await _context.Members
            .Include(bm => bm.User)
            .Where(bm => bm.BoardId == boardId)
            .OrderBy(bm => bm.AddedAt)
            .ToListAsync();
    }

    public async Task<BoardMember> CreateAsync(BoardMember boardMember)
    {
        await _context.Members.AddAsync(boardMember);
        await _context.SaveChangesAsync();
        return boardMember;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var boardMember = await _context.Members
            .FirstOrDefaultAsync(bm => bm.UserId == id);

        if (boardMember == null) return false;

        _context.Members.Remove(boardMember);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsOwnerAsync(Guid boardId, Guid userId)
    {
        var member = await _context.Members
            .FirstOrDefaultAsync(bm => bm.BoardId == boardId && bm.UserId == userId);

        return member != null && member.Role == BoardRole.Owner;
    }
}
