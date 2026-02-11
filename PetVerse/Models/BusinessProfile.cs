namespace PetVerse.Models
{
    public class BusinessProfile : Profile
    {
        public string? IdentificationNumber { get; set; }

        public virtual ICollection<UserToBusinessProfileMapping>? UserToBusinessProfileMapping {get;set;}
    }
}