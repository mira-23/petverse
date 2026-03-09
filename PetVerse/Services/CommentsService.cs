using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Models;
using PetVerse.Queries;

namespace PetVerse.Services
{
    public class CommentsService
    {
        private readonly AppDbContext _context;

        public CommentsService(AppDbContext context)
        {
            _context = context;
        }

        private void ValidateData(CreateCommentDTO dto)
        {
            var errors = new List<string>();
            List<string> types = ["lost", "adoption", "service", "event"];

            if (string.IsNullOrEmpty(dto.Comment) || string.IsNullOrWhiteSpace(dto.Comment))
                errors.Add("Comment is required");

            var postExists = dto.Type switch
            {
                "lost" => _context.LostAnimalPosts.Any(p => p.Id == dto.PostId),
                "adoption" => _context.AnimalAdoptionPosts.Any(p => p.Id == dto.PostId),
                "service" => _context.BusinessPosts.Any(p => p.Id == dto.PostId),
                "event" => _context.EventPosts.Any(p=>p.Id == dto.PostId),
                _ => throw new ValidationException("Invalid type! Type must be: lost|adoption|service|event")
            };

            if (!postExists)
                errors.Add("Post not found");

            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private async Task SaveToDbAsync(Comment comment)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"Database error while creating comment");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"An error occurred while creating the comment: {e.Message}");
            }
        }

        public async Task<Comment> CreateCommentAsync(string userId, CreateCommentDTO dto)
        {
            ValidateData(dto);

            var comment = new Comment
            {
                UserId = userId,
                Content = dto.Comment,
                PostType = dto.Type,
                PublishedAt = DateTime.Now,
                LostAnimalPostId = dto.Type == "lost" ? dto.PostId : null,
                AnimalAdoptionPostId = dto.Type == "adoption" ? dto.PostId : null,
                BusinessPostId = dto.Type == "service" ? dto.PostId : null,
                EventPostId = dto.Type == "event" ? dto.PostId : null
            };

            await SaveToDbAsync(comment);
            return comment;
        }

        internal async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        private async Task<List<CommentResponseDTO>> GetCommentListFromParametersAsync(CommentParameters commentParameters)
        {
            bool postExists = commentParameters.Type switch
            {
                "lost" => await _context.LostAnimalPosts.AnyAsync(p => p.Id == commentParameters.PostId),
                "adoption" => await _context.AnimalAdoptionPosts.AnyAsync(p => p.Id == commentParameters.PostId),
                "service" => await _context.BusinessPosts.AnyAsync(p => p.Id == commentParameters.PostId),
                "event" => await _context.EventPosts.AnyAsync(p => p.Id == commentParameters.PostId),
                _ => false
            };

            if (!postExists)
                throw new KeyNotFoundException();

            return _context.Comments.Where(x =>
                (commentParameters.Type == "lost" && x.LostAnimalPostId == commentParameters.PostId) ||
                (commentParameters.Type == "adoption" && x.AnimalAdoptionPostId == commentParameters.PostId) ||
                (commentParameters.Type == "event" && x.EventPostId == commentParameters.PostId) ||
                (commentParameters.Type == "service" && x.BusinessPostId == commentParameters.PostId))
                .Select(x => new CommentResponseDTO
                {
                    CommentId = x.Id,
                    PostId = commentParameters.PostId,
                    Comment = x.Content,
                    Time = x.PublishedAt,
                    UserId = x.UserId,
                    Type = commentParameters.Type
                })
                .ToList();
        }

        public async Task<List<CommentResponseDTO>> GetPostsAsync(CommentParameters commentParameters)
        {
            return (await GetCommentListFromParametersAsync(commentParameters)).OrderByDescending(x => x.Time).ToList();
        }
    }

}
