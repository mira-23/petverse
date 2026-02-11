using Microsoft.EntityFrameworkCore;
using PetVerse.Entities;

namespace PetVerse.Models
{
    [PrimaryKey(nameof(UserId), nameof(ShelterProfileId))]
    public class UserToShelterProfileMapping
    {
        public string? UserId { get; set; }

        public int ShelterProfileId { get; set; }

        public virtual User? User {get;set;}
        public virtual ShelterProfile? ShelterProfile {get;set;}

    }
}