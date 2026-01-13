using System.ComponentModel.DataAnnotations;

namespace PetVerse.DTOs
{
    public class UserForRegistrationDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        //TEMPORARY! DO NOT STORE PASSWORDS!
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Compare("Password",ErrorMessage = "Password and confirmation password do not match")]
        public required string ConfirmPassword { get; set; }
    }
}
