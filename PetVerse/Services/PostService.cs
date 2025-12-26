using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Interfaces;
using PetVerse.Models;

namespace PetVerse.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreatePostAsync(CreatePostDto dto)
        {
            var post = new Post
            {
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }
    }

}
