namespace PetVerse.DTOs
{
    public class CreateShelterProfileDto
    {
        public required string Address { get; set; }

        public required IFormFile Logo { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public required string IBAN { get; set; }
    }
}
