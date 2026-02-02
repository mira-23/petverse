namespace PetVerse.DTOs
{
    public class CreateBusinessProfileDto
    {
        public required string Address { get; set; }

        public required byte[] Logo { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        public string? IdendificationNumber { get; set; }
    }
}
