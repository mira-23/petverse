using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Models;

namespace PetVerse.Services
{
    public class EngagementService
    {
        private readonly AppDbContext _context;

        public EngagementService(AppDbContext context)
        {
            _context = context;
        }

        private void ValidateData(CreateEngagementDTO dto)
        {
            var errors = new List<string>();

            var validTypes = new[] { "enroll", "interest" };
            if (!validTypes.Contains(dto.Type))
                errors.Add("Type must be either enroll|interest");

            if (!_context.EventPosts.Any(x => x.Id == dto.EventPostId))
                errors.Add("Event post does not exist!");

            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private async Task SaveToDbAsync(Engagement engagement)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(engagement);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    await transaction.RollbackAsync();
                    throw new ValidationException($"This type of engagement ({engagement.Type}) for this event from this user already exists!");
                }
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Database error while creating engagement");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"An error occurred while creating the engagement: {e.Message}");
            }
        }

        public async Task<Engagement> CreateEngagementAsync(string userId, CreateEngagementDTO dto)
        {
            ValidateData(dto);

            var engagement = new Engagement
            {
                EventPostId = dto.EventPostId,
                Type = dto.Type,
                UserId = userId
            };

            await SaveToDbAsync(engagement);
            return engagement;
        }

        public async Task<Engagement?> GetEngagementByIdAsync(int id)
        {
            return await _context.Engagements.FindAsync(id);
        }
    }
}