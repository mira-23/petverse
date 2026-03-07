using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CreateCommentDTO
    {
        public int PostId { get; set; }
        public required string Comment { get; set; }
        [Description("Must be either lost|adoption|service")]
        public required string Type { get; set; }
    }
}