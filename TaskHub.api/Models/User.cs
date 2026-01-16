using System;
using System.ComponentModel.DataAnnotations;

namespace TaskHub.api.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string? UserName { get; set; }
    [Required]
    public byte[]? PasswordHash { get; set; }
    [Required]
    public byte[]? PasswordSalt { get; set; }
    [Required]
    public string? Role { get; set; }


}
