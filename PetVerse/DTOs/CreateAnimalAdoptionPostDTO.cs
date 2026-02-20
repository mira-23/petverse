using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CreateAnimalAdoptionPostDTO : CreateTypedPostDTO
    {
        public int ShelterId { get; set; }
    }
}