namespace PetVerse.Models
{
    public enum Kind
    {
        Dog,
        Cat,
        Other
    }

    public class Pet
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required Kind Kind { get; set; }

        public required DateTime BirthDate { get; set; }
    }
}