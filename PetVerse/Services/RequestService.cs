using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PetVerse.Classes;
using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Models;
using PetVerse.Queries;
using SQLitePCL;

namespace PetVerse.Services
{
    public class RequestService
    {
        private readonly AppDbContext _context;

        public RequestService(AppDbContext context)
        {
            _context = context;
        }

        private void ValidateData(CreateAdoptionRequestDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Message) || string.IsNullOrWhiteSpace(dto.Message))
                errors.Add("Message is required");

            if (!_context.AnimalAdoptionPosts.Any(x=>x.Id == dto.AdoptionPostId))
            {
                errors.Add("Adoption post does not exist!");
            }
            
            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private async Task SaveToDbAsync(AdoptionRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    await transaction.RollbackAsync();
                    throw new ValidationException($"Request for this post from this user already exists!");
                }
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Database error while creating request");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"An error occurred while creating the post: {e.Message}");
            }
        }

        public async Task<AdoptionRequest> CreateAdoptionRequestAsync(string userId, CreateAdoptionRequestDTO dto)
        {
            ValidateData(dto);

            var request = new AdoptionRequest
            {
                AdoptionPostId = dto.AdoptionPostId,
                Message = dto.Message,
                Status = "new",
                UserId = userId
            };

            await SaveToDbAsync(request);
            return request;
        }

        internal async Task<AdoptionRequest?> GetAdoptionRequestByIdAsync(int id)
        {
            return await _context.AdoptionRequests.FindAsync(id);
        }

    }

}
