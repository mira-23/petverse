using PetVerse.Entities;

namespace PetVerse.Models
{
    public class EventPost : Post
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public required string UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Engagement>? Engagements { get; set; }
    }
}

