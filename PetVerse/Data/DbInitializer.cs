using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetVerse.Entities;
using PetVerse.Models;

namespace PetVerse.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var admin = new User
            {
                FirstName = "Administrator",
                LastName = "Administration",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@petverse.com",
                NormalizedEmail = "ADMIN@PETVERSE.COM",
                Pet = new Pet
                {
                    Name = "Buddy",
                    Kind = Kind.Dog,
                    BirthDate = new DateTime(2025, 1, 1)
                },
            };

            var hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, "admin123");

            context.Users.Add(admin);
        }

        context.SaveChanges();
    }
}
