using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CreateSimplePostDTO
    {
        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        public required string Body { get; set; }
    }
}