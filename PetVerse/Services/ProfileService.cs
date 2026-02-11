using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Models;

namespace PetVerse.Services
{
    public class ProfileService
    {
        private readonly AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessProfile> CreateBusinessProfileAsync(CreateBusinessProfileDto createBusinessProfileDto)
        {

            var errors = new List<string>();
        
            if (string.IsNullOrEmpty(createBusinessProfileDto.Name) || string.IsNullOrWhiteSpace(createBusinessProfileDto.Name))
                errors.Add("Name is required");
            
            if (string.IsNullOrEmpty(createBusinessProfileDto.Address) || string.IsNullOrWhiteSpace(createBusinessProfileDto.Address))
                errors.Add("Address is required");

            if (!createBusinessProfileDto.Logo.Any())
                errors.Add("Logo is required");
            
            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));

            using var transaction = await _context.Database.BeginTransactionAsync();
            var businessProfile = new BusinessProfile
                {
                    Address = createBusinessProfileDto.Address,
                    LogoPath = "",
                    Name = createBusinessProfileDto.Name,
                    Description = createBusinessProfileDto.Description,
                    IdentificationNumber = createBusinessProfileDto.IdentificationNumber
                };
            try
            {
                _context.BusinessProfiles.Add(businessProfile);
                await _context.SaveChangesAsync();

                string path = Path.Combine(Environment.CurrentDirectory, @"Images\Logos\",$"{businessProfile.Name}_{businessProfile.Id}.png");
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                File.WriteAllBytes(path, createBusinessProfileDto.Logo);

                businessProfile.LogoPath = path;

                await _context.SaveChangesAsync();
                return businessProfile;
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Database error while creating business profile");
            }
            catch (IOException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error writing logo file to disk");
            }
            catch (ArgumentException)
            {
                await transaction.RollbackAsync();
                throw new ValidationException("Invalid file path or logo data");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"An error occurred while creating the business profile: {e.Message}");
            }
            
        }

        internal async Task<BusinessProfile?> GetBusinessByIdAsync(int id)
        {
            return await _context.BusinessProfiles.FindAsync(id);
        }
    }

}
