namespace PetVerse.DTOs
{
    public class ShelterProfileResponseDTO
    {
        public int Id { get; set; }
        public required string Address { get; set; }

        public required string LogoPath { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public required string IBAN { get; set; }
    }
}
