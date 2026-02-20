using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetVerse.Entities;
using PetVerse.DTOs;
using PetVerse.Services;
using Microsoft.AspNetCore.Authorization;
using PetVerse.Models;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly JwtService _jwtService;

        public AccountsController(UserManager<User> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<RegistratrionResponseDTO>> RegisterUser(UserForRegistrationDTO userForRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new User
            {
                UserName = userForRegistrationDTO.UserName,
                Email = userForRegistrationDTO.Email,
                FirstName = userForRegistrationDTO.FirstName,
                LastName = userForRegistrationDTO.LastName,
                PhoneNumber = userForRegistrationDTO.PhoneNumber
            };

            if (userForRegistrationDTO.Pet != null)
            {
                var pet = new Pet
                {
                    Name = userForRegistrationDTO.Pet.Name,
                    Kind = userForRegistrationDTO.Pet.Kind,
                    BirthDate = userForRegistrationDTO.Pet.BirthDate
                };

                user.Pet = pet;
            }

            var result = await _userManager.CreateAsync(user, userForRegistrationDTO.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistratrionResponseDTO { Error = String.Join(", ", errors.ToArray()) });
            }

            return StatusCode(201);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponseDTO>> Login(UserLoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _jwtService.Authenticate(request);
            if (result is null)
                return Unauthorized();

            return result;
        }

        [AllowAnonymous]
        [HttpGet("verify-username")]
        public async Task<IActionResult> VerifyUserName([FromQuery] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var user = await _userManager.FindByNameAsync(userName);

            return Ok(new { exists = user != null });
        }

    }

}