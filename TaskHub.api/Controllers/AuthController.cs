using System;
using Microsoft.AspNetCore.Mvc;
using TaskHub.api.Dtos;
using TaskHub.api.Services;

namespace TaskHub.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthServices _authService;

    public AuthController(IAuthServices authServices)
    {
        _authService = authServices;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        var result = await _authService.Register(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var token = await _authService.Login(request);
        return Ok(new{Token = token}) ;
    }



}
