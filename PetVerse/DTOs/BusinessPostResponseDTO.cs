using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class BusinessPostRepsonseDTO : PostRepsonseDTO
    {
        public required List<string> MediaPaths { get; set; }
        public int BusinessId { get; set; }
    }
}