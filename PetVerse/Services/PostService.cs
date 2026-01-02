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

        public async Task CreatePostAsync(CreatePostDto dto)
        {
            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // return new CreatePostDto
            // {
            //     Title = post.Title,
            //     Content = post.Content,
            // };
        }
    }

}
