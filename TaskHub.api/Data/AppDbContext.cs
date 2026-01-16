using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskHub.api.Models;

namespace TaskHub.api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet <User> Users { get; set; }




}

