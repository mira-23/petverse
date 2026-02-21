namespace PetVerse.DTOs
{
    public abstract class CreateProfileDto
    {
        public required string Address { get; set; }

        public required IFormFile Logo { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
