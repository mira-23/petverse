namespace PetVerse.Models
{
    public class BusinessPost : Post
    {
        public required ICollection<PostMedia> PostMedias { get; set; }
        public int BusinessProfileId { get; set; }
        public required string UserId { get; set; }
        public required DateTime Published { get; set; }
    }
}
