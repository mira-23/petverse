using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CommentResponseDTO
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public required string Comment { get; set; }
        public required DateTime Time { get; set; }
        public required string UserId { get; set; }
        [Description("Must be either lost|adoption|service")]
        public required string Type { get; set; }
    }
}