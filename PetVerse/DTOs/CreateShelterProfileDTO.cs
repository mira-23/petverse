namespace PetVerse.DTOs
{
    public class CreateShelterProfileDto : CreateProfileDto
    {
        public required string IBAN { get; set; }
    }
}
