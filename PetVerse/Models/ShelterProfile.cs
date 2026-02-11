namespace PetVerse.Models
{
    public class ShelterProfile : Profile
    {
        public required string IBAN { get; set; }

        public virtual ICollection<UserToShelterProfileMapping>? UserToShelterProfileMapping {get;set;}
    }
}