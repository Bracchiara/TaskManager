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

    public async Task<Column?> GetByIdAsync(Guid id)
    {
        return await _context.Columns.FirstOrDefaultAsync(c => c.Id == id);
    }
}
