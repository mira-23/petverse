using Microsoft.AspNetCore.Mvc;
using PetVerse.DTOs;
using PetVerse.Interfaces;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto dto)
        {
            await _postService.CreatePostAsync(dto);
            return Ok("Post created");
        }
    }
}

