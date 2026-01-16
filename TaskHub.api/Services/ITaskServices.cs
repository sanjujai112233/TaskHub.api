using System;
using TaskHub.api.Dtos;

namespace TaskHub.api.Services;

public interface ITaskServices
{
    Task<IEnumerable<TaskReadDto>> GetAllTask();
    Task<TaskReadDto> CreateTask(TaskCreatedDto dto);
    Task CompleteTask(int id, string Status);
    Task<IEnumerable<TaskReadDto>> GetPageTask(
        int pageNumber,
        int pageSize,
        string? status,
        string? priority
    );
}
