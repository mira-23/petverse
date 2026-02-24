using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetVerse.Entities;
using PetVerse.Models;

namespace PetVerse.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }

        public DbSet<ShelterProfile> ShelterProfiles { get; set; }

        public DbSet<UserToShelterProfileMapping> UserToShelterProfileMapping { get; set; }
        public DbSet<UserToBusinessProfileMapping> UserToBusinessProfileMapping { get; set; }

        public DbSet<LostAnimalPost> LostAnimalPosts { get; set; }

        public DbSet<AnimalAdoptionPost> AnimalAdoptionPosts { get; set; }

        public DbSet<BusinessPost> BusinessPosts { get; set; }

        public DbSet<PostMedia> PostMedias { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Buisness Profile Mapping
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

            // Shelter Profile Mapping
            builder.Entity<UserToShelterProfileMapping>()
                .HasKey(e => new { e.UserId, e.ShelterProfileId });

            builder.Entity<UserToShelterProfileMapping>()
                .HasOne(e => e.User)
                .WithMany(u => u.UserToShelterProfileMapping)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserToShelterProfileMapping>()
                .HasOne(e => e.ShelterProfile)
                .WithMany(b => b.UserToShelterProfileMapping)
                .HasForeignKey(e => e.ShelterProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique names for profiles

            builder.Entity<ShelterProfile>()
                .HasIndex(s => s.Name)
                .IsUnique();

            builder.Entity<BusinessProfile>()
                .HasIndex(b => b.Name)
                .IsUnique();

        }


    }
}

