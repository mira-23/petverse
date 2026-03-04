using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
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

        private static void ValidateData(CreateProfileDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrWhiteSpace(dto.Name))
                errors.Add("Name is required");

            if (string.IsNullOrEmpty(dto.Address) || string.IsNullOrWhiteSpace(dto.Address))
                errors.Add("Address is required");

            if (dto.Logo == null
            || dto.Logo.Name == null
            || dto.Logo.FileName == null
            || dto.Logo.Length == 0)
                errors.Add("Logo is required");

            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private async Task SaveToDbAsync(Profile profile, CreateProfileDto dto, string profileType)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(profile);
                await _context.SaveChangesAsync();

                string path = Path.Combine(Environment.CurrentDirectory, "Images", "Logos");
                Directory.CreateDirectory(path);

                string? extension = Path.GetExtension(dto.Logo?.FileName);
                string? name = Path.GetFileNameWithoutExtension(dto.Logo?.FileName);
                string fileName = $"{name}_{profileType}_{profile.Id}{extension}";
                string filePath = Path.Combine(path, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    dto.Logo?.CopyTo(fileStream);
                }

                profile.LogoPath = fileName;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    await transaction.RollbackAsync();
                    throw new ValidationException($"{profileType} profile name already exists!");
                }

                await transaction.RollbackAsync();
                throw new InvalidOperationException($"Database error while creating {profileType} profile");
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
                throw new InvalidOperationException($"An error occurred while creating the {profileType} profile: {e.Message}");
            }
        }

        public async Task<BusinessProfile> CreateBusinessProfileAsync(string userId, CreateBusinessProfileDto createBusinessProfileDto)
        {

            ValidateData(createBusinessProfileDto);

            var businessProfile = new BusinessProfile
            {
                Address = createBusinessProfileDto.Address,
                LogoPath = "",
                Name = createBusinessProfileDto.Name,
                Description = createBusinessProfileDto.Description,
                IdentificationNumber = createBusinessProfileDto.IdentificationNumber
            };

            await SaveToDbAsync(businessProfile, createBusinessProfileDto, "business");

            var userBusinessMapping = new UserToBusinessProfileMapping
            {
                UserId = userId,
                BusinessProfileId = businessProfile.Id
            };

            _context.UserToBusinessProfileMapping.Add(userBusinessMapping);
            await _context.SaveChangesAsync();

            return businessProfile;
        }

        internal async Task<BusinessProfile?> GetBusinessByIdAsync(int id)
        {
            return await _context.BusinessProfiles.FindAsync(id);
        }

        public async Task<ShelterProfile> CreateShelterProfileAsync(string userId, CreateShelterProfileDto createShelterProfileDto)
        {

            ValidateData(createShelterProfileDto);

            if (string.IsNullOrEmpty(createShelterProfileDto.IBAN) || string.IsNullOrWhiteSpace(createShelterProfileDto.IBAN))
                throw new ValidationException("IBAN is required");

            var shelterProfile = new ShelterProfile
            {
                Address = createShelterProfileDto.Address,
                LogoPath = "",
                Name = createShelterProfileDto.Name,
                Description = createShelterProfileDto.Description,
                IBAN = createShelterProfileDto.IBAN
            };

            await SaveToDbAsync(shelterProfile, createShelterProfileDto, "shelter");

            var userShelterMapping = new UserToShelterProfileMapping
            {
                UserId = userId,
                ShelterProfileId = shelterProfile.Id
            };

            _context.UserToShelterProfileMapping.Add(userShelterMapping);
            await _context.SaveChangesAsync();

            return shelterProfile;
        }

        internal async Task<ShelterProfile?> GetShelterByIdAsync(int id)
        {
            return await _context.ShelterProfiles.FindAsync(id);
        }
    }

}
