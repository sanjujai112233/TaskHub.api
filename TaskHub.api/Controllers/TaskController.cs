using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskHub.api.Data;
using TaskHub.api.Dtos;
using TaskHub.api.Models;
using TaskHub.api.Services;

namespace TaskHub.api.Controllers;

[Authorize] // 🔐 THIS MUST BE HERE
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    //Where I write 1st
    //That was 1st. Here i call (Database & Business) logics.
    // but now the Every logic(Database & Business) file different. 

    //private readonly AppDbContext _context; -- 1st
    private readonly ITaskServices _service;

    public TaskController(ITaskServices service)  //AppDbContext context --1st
    {
        // _context = context; -- 1st
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // --1st
        // var tasks = await _context.Tasks.ToListAsync();
        // return Ok(tasks);

        return Ok(await _service.GetAllTask()); // --2nd
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreatedDto task)
    {
        // --1st
        // _context.Tasks.Add(task);
        // await _context.SaveChangesAsync();
        // return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);

        return Ok(await _service.CreateTask(task)); // --2nd
    }

    [Authorize(Roles ="Admin")]
    [HttpPut("{id}/complete")]

    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _service.CompleteTask(id,status); 
        return Ok("Status is updated");
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        int pageNumber = 1,
        int pagedSize = 10,
        string? status = null,
        string? priority = null
    )
    {
        var result = await _service.GetPageTask(
            pageNumber,pagedSize, status, priority
        );

        return Ok(result);
    }

    [HttpPost("{id}/upload")]

    public async Task<IActionResult> UploadFile(
        int id,
        IFormFile file,
        [FromServices] IFileStorageSevice storage
    )
    {
        var url = await storage.UploadAsync(file);
        return Ok(new{Filerurl = url});
    }

}
