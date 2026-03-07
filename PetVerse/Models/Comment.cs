using PetVerse.Entities;

namespace PetVerse.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string PostType { get; set; }
        public required string Content { get; set; }
        public required DateTime PublishedAt { get; set; }
        public required string UserId { get; set; }
        public User? User { get; set; }
        public int? LostAnimalPostId { get; set; }
        public LostAnimalPost? LostAnimalPost { get; set; }
        public int? AnimalAdoptionPostId { get; set; }
        public AnimalAdoptionPost? AnimalAdoptionPost { get; set; }
        public int? BusinessPostId { get; set; }
        public BusinessPost? BusinessPost { get; set; }
    }
}

