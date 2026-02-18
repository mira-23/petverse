using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class LostAnimalPostRepsonseDTO
    {
        public int Id { get; set; }
        public required string PhotoPath { get; set; }
        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
        public required string Body { get; set; }
        public required string UserId { get; set; }
        public required string Status { get; set; }
    }
}

