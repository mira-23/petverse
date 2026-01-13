using System.ComponentModel.DataAnnotations;
using PetVerse.Models;

namespace PetVerse.DTOs
{
    public class PetDTO
    {
        [MinLength(2, ErrorMessage = "LastName is too short")]
        [MaxLength(256, ErrorMessage = "LastName is too long")]
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Pet kind is required, Kind can be: Dog|Cat|Other")]
        public required Kind Kind { get; set; }

        [Required(ErrorMessage = "BirthDate is required")]
        public required DateTime BirthDate { get; set; }
    }
}