using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TaskHub.api.Data;
using TaskHub.api.Models;

namespace TaskHub.api.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }
    public async Task<TaskItem> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }
    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
    }
    public void Update(TaskItem task)
    {
        _context.Tasks.Update(task);
    }
    public void Delete(TaskItem task)
    {
        _context.Tasks.Remove(task);
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<TaskItem>> GetPageAsync(
        int pageNumber,
        int pageSize,
        string? status,
        string? priority)
    {
        var query = _context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(t=> t.Status == status);

        if (!string.IsNullOrEmpty(priority))
        query = query.Where(t=> t.Priority == priority);

        return await query.Take(pageSize).Skip((pageNumber-1)*pageSize).ToListAsync();
    }
}
