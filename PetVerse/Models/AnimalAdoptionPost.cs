namespace PetVerse.Models
{
    public class AnimalAdoptionPost : Post
    {
        public required string PhotoPath { get; set; }
        public required string Type { get; set; }
        public required string ShelterId { get; set; }
        public required string UserId { get; set; }
        public required DateTime Published { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public required string Status { get; set; }
    }
}
