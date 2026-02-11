namespace PetVerse.DTOs
{
    public class BusinessProfileResponseDTO
    {
        public int Id { get; set; }
        public required string Address { get; set; }

        public required string LogoPath { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public string? IdentificationNumber { get; set; }
    }
}
