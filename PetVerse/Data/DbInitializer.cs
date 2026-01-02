using Microsoft.EntityFrameworkCore;
using PetVerse.Models;

namespace PetVerse.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();

        if (context.Posts.Any())
            return;

        context.Posts.AddRange(
            new Post { Title = "Post", Content = "Hello, this is a post" },
            new Post { Title = "Test", Content = "Test test test" }
        );

        context.SaveChanges();
    }
}
