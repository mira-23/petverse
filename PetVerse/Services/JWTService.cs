using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PetVerse.DTOs;
using PetVerse.Entities;

namespace PetVerse.Services
{
    public class JwtService
    {
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserLoginResponseDTO?> Authenticate(UserLoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var userAccount = await _userManager.FindByNameAsync(request.UserName);

            if (userAccount == null)
                return null;

            var userPassword = await _userManager.CheckPasswordAsync(userAccount, request.Password);

            if (!userPassword)
                return null;

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimestamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName)
            }),
                Expires = tokenExpiryTimestamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new UserLoginResponseDTO
            {
                JWTToken = accessToken,
            };
        }
    }
}