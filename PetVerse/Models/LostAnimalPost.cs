namespace PetVerse.Models
{
    public class LostAnimalPost : Post
    {
        public required string PhotoPath { get; set; }
        public required string Type { get; set; }
        public required string UserId { get; set; }
        public required string Status { get; set; }
    }
}

