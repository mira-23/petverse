using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetVerse.DTOs;
using PetVerse.Models;
using PetVerse.Queries;
using PetVerse.Services;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _commentsService;

        public CommentsController(CommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            Comment result;
            try
            {
                result = await _commentsService.CreateCommentAsync(userId, dto);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return StatusCode(500, e.Message);
            }

            if (result == null)
            {
                return BadRequest();
            }

            int postId = 0;
            try
            {
                postId = result.PostType switch
                {
                    "lost" => result.LostAnimalPostId.Value,
                    "adoption" => result.AnimalAdoptionPostId.Value,
                    "service" => result.BusinessPostId.Value,
                    _ => 0
                };
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            var responseDTO = new CommentResponseDTO
            {
                CommentId = result.Id,
                PostId = postId,
                Comment = result.Content,
                Time = result.PublishedAt,
                UserId = result.UserId,
                Type = result.PostType
            };

            return CreatedAtAction(nameof(GetCommentById), new { id = result.Id }, responseDTO);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponseDTO>> GetCommentById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var comment = await _commentsService.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();

            int postId = 0;
            try
            {
                postId = comment.PostType switch
                {
                    "lost" => comment.LostAnimalPostId.Value,
                    "adoption" => comment.AnimalAdoptionPostId.Value,
                    "service" => comment.BusinessPostId.Value,
                    _ => 0
                };
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            var responseDTO = new CommentResponseDTO
            {
                CommentId = comment.Id,
                PostId = postId,
                Comment = comment.Content,
                Time = comment.PublishedAt,
                UserId = comment.UserId,
                Type = comment.PostType
            };
            return Ok(responseDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] CommentParameters commentParameters)
        {
            List<string> postTypes = ["lost", "adoption", "service"];
            if (!postTypes.Contains(commentParameters.Type))
                return BadRequest("Invalid type! Type must be: lost|adoption|service");

            try
            {
                var comments = await _commentsService.GetPostsAsync(commentParameters);
                return Ok(comments);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }
    }
}

