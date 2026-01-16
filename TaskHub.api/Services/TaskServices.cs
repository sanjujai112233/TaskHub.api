using System;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TaskHub.api.Dtos;
using TaskHub.api.Models;
using TaskHub.api.Repositories;
using TaskHub.api.Services;


namespace TaskHub.Api.Services;

public class TaskServices : ITaskServices
{
    private readonly ITaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IEmailServices _email;
    

    public TaskServices(ITaskRepository repo, IMapper mapper, IMemoryCache cache, IEmailServices email)  //Constuctor
    {
        _repo = repo;
        _mapper = mapper;
        _cache = cache;
        _email = email;
    }

    public async Task<IEnumerable<TaskReadDto>> GetAllTask()
    {
        // -- 1st (Without Cache)
        // var task = await _repo.GetAllAsync();
        // return _mapper.Map<IEnumerable<TaskReadDto>>(task);

        // -- 2nd With Cache
        if(!_cache.TryGetValue("tasks", out IEnumerable<TaskReadDto> tasks))
        {
            var data = await _repo.GetAllAsync();
            tasks = _mapper.Map<IEnumerable<TaskReadDto>>(data);

            _cache.Set("tasks", tasks, TimeSpan.FromMinutes(2));
        }
        return tasks;
    }

    public async Task<TaskReadDto> CreateTask(TaskCreatedDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        task.Status = "Pending";
        await _repo.AddAsync(task);
        await _repo.SaveChangesAsync();

        //if in assigned to there is no email. Employee id. 
        // Fetch email through that EmpId in different table.
        //And use here.
        if (!string.IsNullOrEmpty(task.AssignedTo))
        {
            await _email.SendEmailAsync(task.AssignedTo,"New Task Assigned",task.Title);
        }

        return _mapper.Map<TaskReadDto>(task);
    }

    public async Task CompleteTask(int id, string Status)
    {
        var task = await _repo.GetByIdAsync(id);
        if (task == null)
            throw new Exception("Task not foound");

        task.Status = Status;
        _repo.Update(task);
        await _repo.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskReadDto>> GetPageTask(
        int pageNumber,
        int pageSize,
        string? status,
        string? priority
    )
    {
        var task = await _repo.GetPageAsync(pageNumber,pageSize,status,priority);
        return _mapper.Map<IEnumerable<TaskReadDto>>(task);
    }
}
