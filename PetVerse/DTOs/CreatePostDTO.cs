using System.ComponentModel;

namespace PetVerse.DTOs
{
    public abstract class CreatePostDTO : CreateSimplePostDTO
    {
        public required IFormFile Photo { get; set; }
    }
}