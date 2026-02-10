using System.ComponentModel.DataAnnotations;
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
            
            var businessProfile = new BusinessProfile
            {
                Address = createBusinessProfileDto.Address,
                Logo = createBusinessProfileDto.Logo,
                Name = createBusinessProfileDto.Name,
                Description = createBusinessProfileDto.Description,
                IdentificationNumber = createBusinessProfileDto.IdentificationNumber
            };

            _context.BusinessProfiles.Add(businessProfile);
            await _context.SaveChangesAsync();

            return businessProfile;
        }

        internal async Task<BusinessProfile?> GetBusinessByIdAsync(int id)
        {
            return await _context.BusinessProfiles.FindAsync(id);
        }
    }

}
