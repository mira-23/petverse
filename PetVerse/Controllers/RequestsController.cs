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
    [Route("api/adoption_requests")]
    public class RequestsController : ControllerBase
    {
        private readonly RequestService _requestService;

        public RequestsController(RequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAdoptionRequest(CreateAdoptionRequestDTO dto)
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

            AdoptionRequest result;
            try
            {
                result = await _requestService.CreateAdoptionRequestAsync(userId, dto);
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

            var responseDTO = new AdoptionRequestResponseDTO
            {
                Id = result.Id,
                AdoptionPostId = result.AdoptionPostId,
                Message = result.Message,
                Status = result.Status,
                UserName = result.UserName
            };

            return CreatedAtAction(nameof(GetAdoptionRequestById), new { id = result.Id }, responseDTO);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalAdoptionPostRepsonseDTO>> GetAdoptionRequestById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = await _requestService.GetAdoptionRequestByIdAsync(id);
            if (request == null) return NotFound();

            var responseDTO = new AdoptionRequestResponseDTO
            {
                Id = request.Id,
                AdoptionPostId = request.AdoptionPostId,
                Message = request.Message,
                Status = request.Status,
                UserName = request.UserName
            };
            return Ok(responseDTO);
        }
    }
}

