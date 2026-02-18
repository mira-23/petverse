using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CreateLostAnimalPostDTO
    {
        public required IFormFile Photo { get; set; }
        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        public required string Body { get; set; }
        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
    }
}

