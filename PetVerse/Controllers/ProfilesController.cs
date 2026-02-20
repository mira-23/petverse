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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateBusinessProfile(CreateBusinessProfileDto createBusinessProfileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BusinessProfile result;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                result = await _profileService.CreateBusinessProfileAsync(userId,createBusinessProfileDto);
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
                LogoPath = $"{Request.Scheme}://{Request.Host}/Images/Logos/{result.LogoPath}",
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

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var profile = await _profileService.GetBusinessByIdAsync(id);
            if (profile == null) return NotFound();
            
            var responseDTO = new BusinessProfileResponseDTO
            {
                Id = profile.Id,
                Address = profile.Address,
                LogoPath = $"{Request.Scheme}://{Request.Host}/Images/Logos/{profile.LogoPath}",
                Name = profile.Name,
                Description = profile.Description,
                IdentificationNumber = profile.IdentificationNumber
            };
            return Ok(responseDTO);
        }

        [Authorize]
        [HttpPost("shelter")]
        [ProducesResponseType(typeof(CreateShelterProfileDto), StatusCodes.Status201Created)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateShelterProfile(CreateShelterProfileDto createShelterProfileDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ShelterProfile result;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                result = await _profileService.CreateShelterProfileAsync(userId,createShelterProfileDto);
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

            var responseDTO = new ShelterProfileResponseDTO
            {
                Id = result.Id,
                Address = result.Address,
                LogoPath = $"{Request.Scheme}://{Request.Host}/Images/Logos/{result.LogoPath}",
                Name = result.Name,
                Description = result.Description,
                IBAN = result.IBAN
            };

            return CreatedAtAction(nameof(GetShelterById), new { id = responseDTO.Id }, responseDTO);
        }

        [Authorize]
        [HttpGet("shelter/{id}")]
        public async Task<ActionResult<ShelterProfileResponseDTO>> GetShelterById(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var profile = await _profileService.GetShelterByIdAsync(id);
            if (profile == null) return NotFound();
            
            var responseDTO = new ShelterProfileResponseDTO
            {
                Id = profile.Id,
                Address = profile.Address,
                LogoPath = $"{Request.Scheme}://{Request.Host}/Images/Logos/{profile.LogoPath}",
                Name = profile.Name,
                Description = profile.Description,
                IBAN = profile.IBAN
            };
            return Ok(responseDTO);
        }
    }
}
