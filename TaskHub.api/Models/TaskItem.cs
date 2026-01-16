using System;
using System.ComponentModel.DataAnnotations;

namespace TaskHub.api.Models;

public class TaskItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

    public string? Description { get; set; }
    [Required]
    public string Status { get; set; } = "Pending";

    public DateTime DueDate { get; set; }

    [Required]
    public string Priority { get; set; } = "Medium";
    public string? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
