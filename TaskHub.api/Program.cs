using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using TaskHub.api.Data;
using TaskHub.api.Middleware;
using TaskHub.api.Repositories;
using TaskHub.api.Services;
using TaskHub.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------- DbContext ----------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

// ---------------- Serilog ----------------
// builder.Host.UseSerilog((context, config) =>
// {
//     config.ReadFrom.Configuration(context.Configuration);
// });   --1st Method


// --2nd Method
Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("Logs/log-.txt",rollingInterval: RollingInterval.Day)
.CreateLogger();

builder.Host.UseSerilog();
// ---------------- Services ----------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskServices, TaskServices>();

builder.Services.AddMemoryCache(); // --Cache
builder.Services.AddScoped<IEmailServices,EmailServices>();  // --EmailServies
builder.Services.AddScoped<IFileStorageSevice,AzureBlobService>();


// ---------------- Swagger ----------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee Management API",
        Version = "v1",
        Description = "API with JWT Authentication & Authorization"
    });

    // 🔑 Add JWT Security Definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT.\r\n\r\nExample: \"Bearer eyJhbGciOiJI...\""
    });

    // 🔑 Add Security Requirement (applies to all endpoints)
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();

// ---------------- Middleware ----------------
app.UseMiddleware<ExceptionMiddleware>(); //--ExceptionMiddlewre REgistred early in pipeline

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
