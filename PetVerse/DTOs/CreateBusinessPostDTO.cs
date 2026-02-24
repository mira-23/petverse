using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CreateBusinessPostDTO : CreateSimplePostDTO
    {
        public required IFormFileCollection Media {get; set;}
        public int BusinessId { get; set; }
    }
}