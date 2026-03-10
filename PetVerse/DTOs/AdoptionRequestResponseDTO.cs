using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class AdoptionRequestResponseDTO
    {
        public int Id { get; set; }
        public int AdoptionPostId { get; set; }
        public required string UserName { get; set; }
        public required string Message { get; set; }
        [Description("Must be either 'new', 'accepted' or 'rejected'")]
        public required string Status { get; set; }
    }
}