using Microsoft.AspNetCore.Identity;
using PetVerse.Models;

namespace PetVerse.Entities
{
    public class User : IdentityUser
    {
        public required string FirstName {get; set;}

        public required string LastName {get; set;}

        public required Pet Pet {get; set;}
    }
}
