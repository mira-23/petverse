using System.ComponentModel;
using PetVerse.Models;

namespace PetVerse.DTOs
{
    public class AnimalAdoptionPostRepsonseDTO : PostRepsonseDTO
    {
        public int Id { get; set;}
        public required string PhotoPath { get; set; }

        [Description("Must be either 'cat', 'dog' or 'other'")]
        public required string Type { get; set; }
        public int ShelterId { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public required string Status { get; set; }
        public List<AdoptionRequestResponseDTO>? AdoptionRequestResponseDTOs { get; set; }
    }
}