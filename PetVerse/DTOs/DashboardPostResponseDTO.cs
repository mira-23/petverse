using System.ComponentModel;

namespace PetVerse.DTOs
{
    public class DashboardPostRepsonseDTO
    {
        public int? Id { get; set; }
        public required string PostType { get; set; }

        [Description("Must be between 5-128 characters")]
        public required string Title { get; set; }
        public required string Body { get; set; }
        public string? UserId { get; set; }
        public required DateTime Published { get; set; }
        public string? PhotoPath { get; set; }

        [Description("Must be either 'cat', 'dog' or 'other'")]
        public string? Type { get; set; }
        public int? ShelterId { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public string? Status { get; set; }
        public List<string?>? MediaPaths { get; set; }
        public int? BusinessId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}