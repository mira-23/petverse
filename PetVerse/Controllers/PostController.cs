using Microsoft.AspNetCore.Mvc;
using PetVerse.DTOs;
using PetVerse.Services;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto dto)
        {
            await _postService.CreatePostAsync(dto);
            return Ok(new
            {
                success = true,
                message = "Post created successfully",
                post = new
                {
                    title = dto.Title,
                    content = dto.Content
                }
            });
        }
    }
}

