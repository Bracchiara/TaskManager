using Microsoft.EntityFrameworkCore;
using TaskManager.Data.DBContext;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<TaskItem?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Tasks
            .Include (t => t.Column)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }
    public async Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var task = await GetByIdAsync(id, userId);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }  
}
