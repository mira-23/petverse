using PetVerse.Entities;

namespace PetVerse.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public required string PostType { get; set; }
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public required DateTime PublishedAt { get; set; }
        public AnimalAdoptionPost? AnimalAdoptionPost { get; set; }
        public LostAnimalPost? LostAnimalPost { get; set; }
        public BusinessPost? BusinessPost { get; set; }
        public User? User { get; set; }
    }
}

