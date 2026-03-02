namespace PetVerse.Models
{
    public abstract class Post
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Body { get; set; }

        public required DateTime Published { get; set; }
    }
}

