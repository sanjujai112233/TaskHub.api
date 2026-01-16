using System;

namespace TaskHub.api.Dtos;

public class UserRegisterDto
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; } = "User";

}
