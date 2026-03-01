using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class LostAnimalPostRepsonseDTO : PostRepsonseDTO
    {
        public int Id { get; set; }
        public required string PhotoPath { get; set; }
        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
        public required string Status { get; set; }
    }
}

