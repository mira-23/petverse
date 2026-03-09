using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class CreateEngagementDTO
    {
        public int EventPostId { get; set; }
        [Description("Must be either enroll|interest")]
        public required string Type { get; set; }
    }
}