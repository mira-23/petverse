namespace PetVerse.DTOs
{
    public class CreateEventPostDTO : CreateSimplePostDTO
    {
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }
}