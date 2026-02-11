using Swashbuckle.AspNetCore.Annotations;

namespace PetVerse.DTOs
{
    public class CreateBusinessProfileDto
    {
        public required string Address { get; set; }

        public required IFormFile Logo { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public string? IdentificationNumber { get; set; }
    }
}
