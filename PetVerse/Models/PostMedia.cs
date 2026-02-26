namespace PetVerse.Models
{
    public class PostMedia
    {
        public int Id { get; set; }

        public required string Path { get; set; }
        public int BusinessPostId { get; set; }
    }
}