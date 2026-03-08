namespace PetVerse.DTOs
{
    public class EventPostResponseDTO : PostRepsonseDTO
    {
        public required int Id { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }
}