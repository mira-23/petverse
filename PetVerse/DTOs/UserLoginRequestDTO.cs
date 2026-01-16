namespace PetVerse.DTOs
{
    public class UserLoginRequestDTO
    {
        public required string UserName {get; set;}

        public required string Password {get; set;}
    }
}