namespace PetVerse.DTOs
{
    public class EngagementResponseDTO
    {
        public int Id { get; set; }
        public int EventPostId { get; set; }
        public required string Type { get; set; }
        public required string UserId { get; set; }
    }
}