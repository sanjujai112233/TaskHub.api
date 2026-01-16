using System;
using System.Globalization;
using TaskHub.api.Models;


namespace TaskHub.api.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem> GetByIdAsync(int id);
    Task AddAsync(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<TaskItem>> GetPageAsync(
        int pageNumber,
        int pageSize,
        string? status,
        string? priority
    );
}
