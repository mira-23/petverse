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
    [Route("api/engagements")]
    public class EngagementsController : ControllerBase
    {
        private readonly EngagementService _engagementService;

        public EngagementsController(EngagementService engagementService)
        {
            _engagementService = engagementService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEngagement(CreateEngagementDTO dto)
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

            Engagement result;
            try
            {
                result = await _engagementService.CreateEngagementAsync(userId, dto);
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

            var responseDTO = new EngagementResponseDTO
            {
                Id = result.Id,
                EventPostId = result.EventPostId,
                Type = result.Type,
                UserId = result.UserId
            };

            return CreatedAtAction(nameof(GetEngagementById), new { id = result.Id }, responseDTO);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EngagementResponseDTO>> GetEngagementById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var engagement = await _engagementService.GetEngagementByIdAsync(id);
            if (engagement == null) return NotFound();

            var responseDTO = new EngagementResponseDTO
            {
                Id = engagement.Id,
                EventPostId = engagement.EventPostId,
                Type = engagement.Type,
                UserId = engagement.UserId
            };

            return Ok(responseDTO);
        }
    }
}