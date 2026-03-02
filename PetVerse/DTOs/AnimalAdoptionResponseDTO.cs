using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class AnimalAdoptionPostRepsonseDTO : PostRepsonseDTO
    {
        public required string PhotoPath { get; set; }

        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
        public int ShelterId { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public required string Status { get; set; }
    }
}