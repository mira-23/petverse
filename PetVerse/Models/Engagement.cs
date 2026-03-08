using PetVerse.Entities;

namespace PetVerse.Models
{
    public class Engagement
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public int EventPostId { get; set; }
        public EventPost? EventPost { get; set; }
        public required string UserId { get; set; }
        public User? User { get; set; }
    }
}