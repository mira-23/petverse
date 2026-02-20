using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class AnimalAdoptionPostRepsonseDTO
    {
        public required string PhotoPath { get; set; }
        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        public required string Body { get; set; }
        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
        public required string ShelterId { get; set; }
        public required string UserId { get; set; }
        public required DateTime Published { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public required string Status { get; set; }
    }
}