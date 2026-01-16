using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskHub.api.Data;
using TaskHub.api.Dtos;
using TaskHub.api.Models;

namespace TaskHub.api.Services;

public interface IAuthServices
{
    Task<string> Register(UserRegisterDto request);
    Task<string> Login(LoginDto request);

}
public class AuthServices : IAuthServices
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthServices(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> Register(UserRegisterDto request)
    {
        // if (_context.Users.Any(u => u.UserName == request.UserName))
        //     throw new Exception("User Already Exists");
        if (_context.Users.Any(u => u.UserName == request.UserName))
            throw new Exception("Username already exists");

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            UserName = request.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = request.Role,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return "User Register Successfully";

    }

    public async Task<string> Login(LoginDto request)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName);

        if (user == null)
            throw new Exception("User not found");

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Invalid Password");

        return CreateToken(user);
    }

    private string CreateToken(User user)
    {

        var claims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, user.UserName),
          new Claim(ClaimTypes.Role, user.Role),

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
    issuer: _config["Jwt:Issuer"],      
    audience: _config["Jwt:Audience"],  
    claims: claims,
    expires: DateTime.UtcNow.AddHours(1),
    signingCredentials: creds
);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

    }

    private bool VerifyPasswordHash(string password, byte[] storeHash, byte[] storeSalt)
    {
        using var hmac = new HMACSHA512(storeSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(storeHash);
    }


}
