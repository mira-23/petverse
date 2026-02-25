using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class BusinessPostRepsonseDTO
    {
        public required List<string> MediaPaths { get; set; }
        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        public required string Body { get; set; }
        public int BusinessId { get; set; }
        public required string UserId { get; set; }
        public required DateTime Published { get; set; }
    }
}