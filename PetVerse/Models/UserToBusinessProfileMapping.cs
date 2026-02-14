using Microsoft.EntityFrameworkCore;
using PetVerse.Entities;

namespace PetVerse.Models
{
    [PrimaryKey(nameof(UserId), nameof(BusinessProfileId))]
    public class UserToBusinessProfileMapping
    {
        public string? UserId { get; set; }

        public int BusinessProfileId { get; set; }

        public virtual User? User {get;set;}
        public virtual BusinessProfile? BusinessProfile {get;set;}

    }
}