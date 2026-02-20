namespace PetVerse.Models
{
    public class LostAnimalPost : PhotoPost
    {
        public required string Type { get; set; }
        public required string UserId { get; set; }
        public required string Status { get; set; }
    }
}

