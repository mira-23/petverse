using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
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
        private readonly IMapper _mapper;

        private readonly JwtService _jwtService;

        public AccountsController(UserManager<User> userManager, IMapper mapper, JwtService jwtService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("register")] 
        public async Task<ActionResult<RegistratrionResponseDTO>> RegisterUser([FromBody] UserForRegistrationDTO userForRegistrationDTO)
        {
            if(userForRegistrationDTO == null)
            {
                return BadRequest();
            }

            var pet = new Pet
            {
                Name = userForRegistrationDTO.Pet.Name,
                Kind = userForRegistrationDTO.Pet.Kind,
                BirthDate = userForRegistrationDTO.Pet.BirthDate
            };

            var user = new User
            {
                UserName = userForRegistrationDTO.UserName,
                Email = userForRegistrationDTO.Email,
                FirstName = userForRegistrationDTO.FirstName,
                LastName = userForRegistrationDTO.LastName,
                PhoneNumber = userForRegistrationDTO.PhoneNumber,
                Pet = pet
            };
            var result = await _userManager.CreateAsync(user,userForRegistrationDTO.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistratrionResponseDTO {Error = String.Join(", ", errors.ToArray())});
            }

            return StatusCode(201);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponseDTO>> Login(UserLoginRequestDTO request)
        {
            var result = await _jwtService.Authenticate(request);
            if (result is null)
                return Unauthorized();

            return result;
        }

        [HttpGet("verify-username")]
        public async Task<IActionResult> VerifyUserName([FromQuery] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return Ok(new { exists = user != null });
        }

        }
   
}