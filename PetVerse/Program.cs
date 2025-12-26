using Microsoft.EntityFrameworkCore;
using PetVerse.Data;
using PetVerse.Interfaces;
using PetVerse.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// In-memory database instead of SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MockDb"));

// Services
builder.Services.AddScoped<IPostService, PostService>();

// Swagger (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
