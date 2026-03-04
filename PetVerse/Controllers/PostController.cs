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
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        private readonly IAuthorizationService _authorizationService;

        public PostsController(PostService postService, IAuthorizationService authorizationService)
        {
            _postService = postService;
            _authorizationService = authorizationService;
        }

        [HttpPost("user/lost_animal")]
        public async Task<IActionResult> CreateLostAnimalPost(CreateLostAnimalPostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            dto,
            "IsUser");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            LostAnimalPost result;
            try
            {
                result = await _postService.CreateLostAnimalPostAsync(userId, dto);
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

            var responseDTO = new LostAnimalPostRepsonseDTO
            {
                Id = result.Id,
                PhotoPath = $"{Request.Scheme}://{Request.Host}/Images/LostAnimals/{result.PhotoPath}",
                Title = result.Title,
                Type = result.Type,
                Body = result.Body,
                UserId = result.UserId,
                Status = result.Status,
                Published = result.Published
            };

            return CreatedAtAction(nameof(GetLostAnimalPostById), new { id = responseDTO.Id }, responseDTO);
        }

        [HttpGet("user/lost_animal/{id}")]
        public async Task<ActionResult<LostAnimalPostRepsonseDTO>> GetLostAnimalPostById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
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
                Status = post.Status,
                Published = post.Published
            };
            return Ok(responseDTO);
        }

        [HttpPost("shelter/animal_adoption")]
        public async Task<IActionResult> CreateAnimalAdoptionPost(CreateAnimalAdoptionPostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            dto,
            "IsShelter");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            AnimalAdoptionPost result;
            try
            {
                result = await _postService.CreateAnimalAdoptionPostAsync(userId, dto);
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

            var responseDTO = new AnimalAdoptionPostRepsonseDTO
            {
                PhotoPath = $"{Request.Scheme}://{Request.Host}/Images/AnimalAdoptions/{result.PhotoPath}",
                Title = result.Title,
                Type = result.Type,
                Body = result.Body,
                ShelterId = result.ShelterProfileId,
                UserId = result.UserId,
                Published = result.Published,
                AdoptedAt = result.AdoptedAt,
                Status = result.Status
            };

            return CreatedAtAction(nameof(GetAnimalAdoptionPostById), new { id = result.Id }, responseDTO);
        }

        [HttpGet("shelter/animal_adoption/{id}")]
        public async Task<ActionResult<AnimalAdoptionPostRepsonseDTO>> GetAnimalAdoptionPostById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var post = await _postService.GetAnimalAdoptionPostByIdAsync(id);
            if (post == null) return NotFound();

            var responseDTO = new AnimalAdoptionPostRepsonseDTO
            {
                PhotoPath = $"{Request.Scheme}://{Request.Host}/Images/AnimalAdoptions/{post.PhotoPath}",
                Title = post.Title,
                Type = post.Type,
                Body = post.Body,
                ShelterId = post.ShelterProfileId,
                UserId = post.UserId,
                Published = post.Published,
                AdoptedAt = post.AdoptedAt,
                Status = post.Status
            };
            return Ok(responseDTO);
        }

        [HttpPost("business/business_post")]
        public async Task<IActionResult> CreateBusinessPost(CreateBusinessPostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
            User,
            dto,
            "IsBusiness");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            BusinessPost result;
            try
            {
                result = await _postService.CreateBusinessPostAsync(userId, dto);
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

            var responseDTO = new BusinessPostRepsonseDTO
            {
                BusinessId = result.BusinessProfileId,
                MediaPaths = [.. result.PostMedias.Select(x=>$"{Request.Scheme}://{Request.Host}/Images/Businesss/{x.Path}")],
                Title = result.Title,
                Body = result.Body,
                UserId = result.UserId,
                Published = result.Published
            };

            return CreatedAtAction(nameof(GetBusinessPostById), new { id = result.Id }, responseDTO);
        }

        [HttpGet("business/business_post/{id}")]
        public async Task<ActionResult<BusinessPostRepsonseDTO>> GetBusinessPostById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var post = await _postService.GetBusinessPostByIdAsync(id);
            if (post == null) return NotFound();

            var responseDTO = new BusinessPostRepsonseDTO
            {
                BusinessId = post.BusinessProfileId,
                MediaPaths = [.. post.PostMedias.Select(x=>$"{Request.Scheme}://{Request.Host}/Images/Businesss/{x.Path}")],
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Published = post.Published
            };

            return Ok(responseDTO);
        }

        [HttpGet]
        public IActionResult GetPosts([FromQuery] PostParameters postParameters)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = _postService.GetPosts(postParameters,userId,$"{Request.Scheme}://{Request.Host}");
            return Ok(posts);
        }

        [HttpPut("user/lost_animal/{id}")]
        public async Task<IActionResult> MarkLostAnimalAsFound(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            FoundAnimalPostRepsonseDTO responseDTO;
            try
            {
                responseDTO = await _postService.MarkLostAnimalAsFound(id, userId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return Forbid();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException e)
            {
                return StatusCode(500, e.Message);
            }

            string fileName = responseDTO.PhotoPath;
            responseDTO.PhotoPath = $"{Request.Scheme}://{Request.Host}/Images/LostAnimals/{fileName}";
            return Ok(responseDTO);
        }
    }
}

