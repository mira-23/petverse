namespace PetVerse.Models
{
    public class BusinessProfile : Profile
    {
        public string? IdentificationNumber { get; set; }

        public virtual ICollection<UserToBusinessProfileMapping>? UserToBusinessProfileMapping {get;set;}
        public ICollection<BusinessPost>? BusinessPosts { get; set; }
    }
}