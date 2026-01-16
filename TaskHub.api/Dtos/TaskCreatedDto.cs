using System;

namespace TaskHub.api.Dtos;

public class TaskCreatedDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string DueDate { get; set; }
    public string Priority { get; set; }
    public string AssignedTo { get; set; }
}
