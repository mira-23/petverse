using Microsoft.EntityFrameworkCore;
using PetVerse.Data;
using PetVerse.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// In-memory database instead of SQLite
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("MockDb"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Services
builder.Services.AddScoped<PostService>();

// Swagger (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
