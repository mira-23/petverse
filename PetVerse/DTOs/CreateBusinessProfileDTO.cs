using Swashbuckle.AspNetCore.Annotations;

namespace PetVerse.DTOs
{
    public class CreateBusinessProfileDto
    {
        public required string Address { get; set; }

        [SwaggerSchema("Logo image in base64 format (example:iVBORw0KGgoAAAANSUhEUgAA...)", Format = "byte")]
        public required byte[] Logo { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public string? IdentificationNumber { get; set; }
    }
}
