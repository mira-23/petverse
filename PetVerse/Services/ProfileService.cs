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

            if (createBusinessProfileDto.Logo == null 
            || createBusinessProfileDto.Logo.Name == null 
            || createBusinessProfileDto.Logo.FileName == null
            || createBusinessProfileDto.Logo.Length==0)
                errors.Add("Logo is required");
            
            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));

            var businessProfile = new BusinessProfile
                {
                    Address = createBusinessProfileDto.Address,
                    LogoPath = "",
                    Name = createBusinessProfileDto.Name,
                    Description = createBusinessProfileDto.Description,
                    IdentificationNumber = createBusinessProfileDto.IdentificationNumber
                };
                
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.BusinessProfiles.Add(businessProfile);
                await _context.SaveChangesAsync();

                string path = Path.Combine(Environment.CurrentDirectory, "Images","Logos");
                Directory.CreateDirectory(path);
                
                string extension = Path.GetExtension(createBusinessProfileDto.Logo.FileName);
                string name = Path.GetFileNameWithoutExtension(createBusinessProfileDto.Logo.FileName);
                string fileName = $"{name}_{businessProfile.Id}{extension}";
                string filePath = Path.Combine(path, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    createBusinessProfileDto.Logo.CopyTo(fileStream);
                }

                businessProfile.LogoPath = fileName;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
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
                throw new InvalidOperationException("Error saving logo file to disk");
            }
            catch (ArgumentException)
            {
                await transaction.RollbackAsync();
                throw new ValidationException("Invalid file path (name possibly has invalid characters)");
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
