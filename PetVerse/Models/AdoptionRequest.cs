using PetVerse.Entities;

namespace PetVerse.Models
{
    public class AdoptionRequest
    {
        public int Id { get; set; }
        public int AdoptionPostId { get; set; }
        public required string Message { get; set; }
        public required string Status { get; set; }
        public required string UserName { get; set; }
        public required string UserId {get; set; }
        public virtual User? User {get;set;}
        public AnimalAdoptionPost? AnimalAdoptionPost { get; set; }
    }
}

