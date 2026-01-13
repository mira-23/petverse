using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PetVerse.Entities;
using PetVerse.DTOs;

namespace PetVerse.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("register")] 
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDTO userForRegistrationDTO)
        {
            if(userForRegistrationDTO == null)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(userForRegistrationDTO);
            var result = await _userManager.CreateAsync(user,userForRegistrationDTO.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistratrionResponseDTO {Errors = errors});
            }

            return StatusCode(201);
        }
    }
}