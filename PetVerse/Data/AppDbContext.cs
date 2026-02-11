using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetVerse.Entities;
using PetVerse.Models;

namespace PetVerse.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

             
        }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserToBusinessProfileMapping>()
                .HasKey(e => new { e.UserId, e.BusinessProfileId });
            
            builder.Entity<UserToBusinessProfileMapping>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserToBusinessProfileMapping)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserToBusinessProfileMapping>()
                .HasOne(e => e.BusinessProfile)
                .WithMany(b => b.UserToBusinessProfileMapping)
                .HasForeignKey(e => e.BusinessProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}

