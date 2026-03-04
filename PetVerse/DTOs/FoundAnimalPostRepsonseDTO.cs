namespace PetVerse.DTOs
{
    public class FoundAnimalPostRepsonseDTO
    {
        public int Id { get; set; }
        public required string PhotoPath { get; set; }
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required string UserId { get; set; }
        public required string Status { get; set; }
    }
}
