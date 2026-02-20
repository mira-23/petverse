using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Models;

namespace PetVerse.Services
{
    public class PostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        private void ValidateData(CreateTypedPostDTO dto)
        {
            List<string> animalTypes = ["cat", "dog", "other"];
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Title) || string.IsNullOrWhiteSpace(dto.Title))
                errors.Add("Title is required");

            if (dto.Title.Length > 128 || dto.Title.Length < 5)
            {
                errors.Add("Title must be between 5 and 128 characters");
            }

            if (string.IsNullOrEmpty(dto.Body) || string.IsNullOrWhiteSpace(dto.Body))
                errors.Add("Post content (body) is required");

            if (dto.Photo == null
            || dto.Photo.Name == null
            || dto.Photo.FileName == null
            || dto.Photo.Length == 0)
                errors.Add("Photo is required");

            if (string.IsNullOrEmpty(dto.Type) || string.IsNullOrWhiteSpace(dto.Type))
                errors.Add("Animal type is required");

            if (!animalTypes.Contains(dto.Type))
                errors.Add("Animal type must be cat|dog|other");

            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private async Task SaveToDbAsync(PhotoPost post, CreateTypedPostDTO dto, string photoType)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(post);

                await _context.SaveChangesAsync();

                string path = Path.Combine(Environment.CurrentDirectory, "Images", $"{photoType}s");
                Directory.CreateDirectory(path);

                string extension = Path.GetExtension(dto.Photo.FileName);
                string name = Path.GetFileNameWithoutExtension(dto.Photo.FileName);
                string fileName = $"{name}_{photoType}_{post.Id}{extension}";
                string filePath = Path.Combine(path, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await dto.Photo.CopyToAsync(fileStream);
                }

                post.PhotoPath = fileName;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Database error while creating post");
            }
            catch (IOException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error saving animal photo file to disk");
            }
            catch (ArgumentException)
            {
                await transaction.RollbackAsync();
                throw new ValidationException("Invalid file path (name possibly has invalid characters)");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"An error occurred while creating the post: {e.Message}");
            }
        }

        public async Task<LostAnimalPost> CreateLostAnimalPostAsync(string userId, CreateLostAnimalPostDTO dto)
        {
            ValidateData(dto);

            var post = new LostAnimalPost
            {
                PhotoPath = "",
                Title = dto.Title,
                Body = dto.Body,
                Type = dto.Type,
                UserId = userId,
                Status = "notFound"
            };

            string photoType = "LostAnimal";

            await SaveToDbAsync(post, dto, photoType);
            return post;
        }

        internal async Task<LostAnimalPost?> GetLostAnimalPostByIdAsync(int id)
        {
            return await _context.LostAnimalPosts.FindAsync(id);
        }

        public async Task<AnimalAdoptionPost> CreateLostAnimalPostAsync(string userId, CreateAnimalAdoptionPostDTO dto)
        {
            ValidateData(dto);

            var post = new AnimalAdoptionPost
            {
                PhotoPath = "",
                Title = dto.Title,
                Body = dto.Body,
                Type = dto.Type,
                ShelterProfileId = dto.ShelterId,
                UserId = userId,
                Published = DateTime.Now,
                AdoptedAt = null,
                Status = "available"
            };

            string photoType = "AnimalAdoption";

            await SaveToDbAsync(post, dto, photoType);
            return post;
        }

        internal async Task<AnimalAdoptionPost?> GetAnimalAdoptionPostByIdAsync(int id)
        {
            return await _context.AnimalAdoptionPosts.FindAsync(id);
        }
    }

}
