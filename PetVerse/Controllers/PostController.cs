using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetVerse.DTOs;
using PetVerse.Models;
using PetVerse.Services;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        private readonly IAuthorizationService _authorizationService;

        public PostsController(PostService postService,IAuthorizationService authorizationService)
        {
            _postService = postService;
            _authorizationService = authorizationService;
        }

        [HttpPost("user/lost_animal")]
        public async Task<IActionResult> CreateLostAnimalPost(CreateLostAnimalPostDTO dto)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            dto,
            "IsUser");
        
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LostAnimalPost result;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                result = await _postService.CreateLostAnimalPostAsync(userId,dto);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            } 
            catch (InvalidOperationException e)
            {
                return StatusCode(500,e.Message);
            } 

            if (result == null)
            {
                return BadRequest();
            }

            var responseDTO = new LostAnimalPostRepsonseDTO
            {
                Id = result.Id,
                PhotoPath = $"{Request.Scheme}://{Request.Host}/Images/LostAnimals/{result.PhotoPath}",
                Title = result.Title,
                Type = result.Type,
                Body = result.Body,
                UserId = result.UserId,
                Status = result.Status
            };

            return CreatedAtAction(nameof(GetLostAnimalPostById), new { id = responseDTO.Id }, responseDTO);
        }

        [HttpGet("user/lost_animal/{id}")]
        public async Task<ActionResult<LostAnimalPostRepsonseDTO>> GetLostAnimalPostById(int id)
        {
            var post = await _postService.GetLostAnimalPostByIdAsync(id);
            if (post == null) return NotFound();
            
            var responseDTO = new LostAnimalPostRepsonseDTO
            {
                Id = post.Id,
                PhotoPath = $"{Request.Scheme}://{Request.Host}/Images/LostAnimals/{post.PhotoPath}",
                Title = post.Title,
                Type = post.Type,
                Body = post.Body,
                UserId = post.UserId,
                Status = post.Status
            };
            return Ok(responseDTO);
        }
    }
}

