namespace PetVerse.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public required string PostType { get; set; }
        public required string UserId { get; set; }
        public required string Content {get; set; }
        public required string PublishedAt { get; set; }
    }
}

