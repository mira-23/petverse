using System.ComponentModel;

namespace PetVerse.DTOs
{
    public abstract class CreateTypedPostDTO : CreatePostDTO
    {
        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
    }
}