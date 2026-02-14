using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PetVerse.DTOs
{
    public class UserForRegistrationDTO
    {
        [MinLength(2, ErrorMessage = "Username is too short")]
        [MaxLength(256, ErrorMessage = "Username is too long")]
        [Remote(action: "VerifyUserName", controller: "AccountsController")]
        [Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }

        [MinLength(2, ErrorMessage = "FirstName is too short")]
        [MaxLength(256, ErrorMessage = "FirstName is too long")]
        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "LastName is too short")]
        [MaxLength(256, ErrorMessage = "LastName is too long")]
        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "Password is too short")]
        [MaxLength(256, ErrorMessage = "Password is too long")]
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        public PetDTO? Pet { get; set; }
    }
}
