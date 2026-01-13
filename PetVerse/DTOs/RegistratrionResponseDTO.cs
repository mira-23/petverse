namespace PetVerse.DTOs
{
    public class RegistratrionResponseDTO
    {
        public bool IsSuccessfulRegistration { get; set; }
        public required IEnumerable<string> Errors { get; set; }
    }
}
