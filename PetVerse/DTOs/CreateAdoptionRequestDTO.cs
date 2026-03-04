namespace PetVerse.DTOs
{
    public class CreateAdoptionRequestDTO
    {
        public int AdoptionPostId { get; set; }
        public required string Message { get; set; }
    }
}