using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetVerse.DTOs;
using PetVerse.Models;
using PetVerse.Services;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfilesController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [Authorize]
        [HttpPost("business")]
        [ProducesResponseType(typeof(CreateBusinessProfileDto), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateBusinessProfile(CreateBusinessProfileDto createBusinessProfileDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BusinessProfile result;

            try
            {
                result = await _profileService.CreateBusinessProfileAsync(createBusinessProfileDto);
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

            var responseDTO = new BusinessProfileResponseDTO
            {
                Id = result.Id,
                Address = result.Address,
                LogoPath = result.LogoPath,
                Name = result.Name,
                Description = result.Description,
                IdentificationNumber = result.IdentificationNumber
            };

            return CreatedAtAction(nameof(GetBusinessById), new { id = responseDTO.Id }, responseDTO);
        }

        [Authorize]
        [HttpGet("business/{id}")]
        public async Task<ActionResult<BusinessProfileResponseDTO>> GetBusinessById(int id)
        {
            var profile = await _profileService.GetBusinessByIdAsync(id);
            if (profile == null) return NotFound();
            
            var responseDTO = new BusinessProfileResponseDTO
            {
                Id = profile.Id,
                Address = profile.Address,
                LogoPath = profile.LogoPath,
                Name = profile.Name,
                Description = profile.Description,
                IdentificationNumber = profile.IdentificationNumber
            };
            return Ok(responseDTO);
        }
    }
}
