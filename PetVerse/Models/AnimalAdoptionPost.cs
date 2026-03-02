namespace PetVerse.Models
{
    public class AnimalAdoptionPost : PhotoPost
    {
        public required string Type { get; set; }
        public int ShelterProfileId { get; set; }
        public required string UserId { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public required string Status { get; set; }
    }
}
