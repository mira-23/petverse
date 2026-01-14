namespace PetVerse.DTOs
{
    public class RegistratrionResponseDTO
    {
        public bool IsSuccessfulRegistration { get; set; }
        public required string Error { get; set; }
    }
}
