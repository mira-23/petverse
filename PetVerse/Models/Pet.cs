using System.ComponentModel.DataAnnotations;

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

    //   "pet": {
//     "name": "string:2-256",
//     "kind": "list:TDB",
//      "birthDate": "ISO-Date (2026-01-08)"
//      "photo: "binary"
}