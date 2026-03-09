namespace PetVerse.Models
{
    public class Pet
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Kind { get; set; }

        public required DateTime BirthDate { get; set; }
    }
}