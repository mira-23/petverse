using System.ComponentModel;

namespace PetVerse.DTOs
{
    public abstract class PostRepsonseDTO
    {
        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required string UserId { get; set; }
        public required DateTime Published { get; set; }
    }
}