using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class AnimalAdoptionPostRepsonseDTO : CreateTypedPostDTO
    {
        public required string PhotoPath { get; set; }
        [Description("Must be between 5-128 characters")]
        public int ShelterId { get; set; }
        public required string UserId { get; set; }
        public required DateTime Published { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public required string Status { get; set; }
    }
}